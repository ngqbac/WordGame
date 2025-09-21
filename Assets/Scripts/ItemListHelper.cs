using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArcherGame.Game3.Helper
{
    public static class ItemListHelper
    {
        public static ItemListBuilder<T> For<T>(List<T> itemList) where T : Component => new(itemList);

        public static List<T> BuildList<T>(GameObject itemPrefab, Transform parent) where T : Component
        {
            // clear parent children
            foreach (Transform child in parent)
            {
                Object.Destroy(child.gameObject);
            }
            // add itemPrefab
            var itemList = new List<T>();
            var item = Object.Instantiate(itemPrefab, parent).GetComponent<T>();
            itemList.Add(item);
            return new List<T>(itemList);
        }

        #region Internal adjust

        public static void Adjust<T>(
            List<T> itemList, int desiredCount,
            Action<T, int> configureItem = null,
            Action onComplete = null,
            Action<T, int> onCompleteConfigure = null,
            EGameObjectState objectState = EGameObjectState.Hidden) where T : Component
        {
            if (itemList.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var baseItem = itemList[0];
            baseItem.Hide();
            var parent = baseItem.transform.parent;

            AdjustItemCount(itemList, baseItem, desiredCount, parent);
            ApplyObjectState(itemList, desiredCount, objectState);

            if (configureItem == null)
            {
                OnComplete(parent as RectTransform, onComplete);
                return;
            }

            for (var i = 1; i <= desiredCount; i++)
            {
                configureItem.Invoke(itemList[i], i - 1);
                itemList[i].Show();
                onCompleteConfigure?.Invoke(itemList[i], i - 1);
            }

            OnComplete(parent as RectTransform, onComplete);
        }

        public static async Task AdjustAsync<T>(
            List<T> itemList, int desiredCount,
            Func<T, int, Task> configureItem = null,
            Action onComplete = null,
            EGameObjectState objectState = EGameObjectState.Hidden) where T : Component
        {
            if (itemList.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var baseItem = itemList[0];
            baseItem.Hide();
            var parent = baseItem.transform.parent;

            AdjustItemCount(itemList, baseItem, desiredCount, parent);
            ApplyObjectState(itemList, desiredCount, objectState);

            if (configureItem == null)
            {
                OnComplete(parent as RectTransform, onComplete);
                return;
            }

            await Task.WhenAll(itemList
                .Skip(1)
                .Take(desiredCount)
                .Select(async (item, i) =>
                {
                    await configureItem(item, i);
                    item.Show();
                })
            ).ContinueWith(_ =>
            {
                OnComplete(parent as RectTransform, onComplete);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void AdjustItemCount<T>(List<T> itemList, T baseItem, int desiredCount, Transform parent) where T : Component
        {
            while (itemList.Count - 1 < desiredCount)
            {
                var item = Object.Instantiate(baseItem, parent, false);
                item.name = $"{baseItem.name}_{itemList.Count - 1}";
                itemList.Add(item);
            }
        }

        private static void ApplyObjectState<T>(List<T> itemList, int desiredCount, EGameObjectState objectState) where T : Component
        {
            for (var i = desiredCount + 1; i < itemList.Count; i++)
            {
                itemList[i].Hide();
                if (objectState.HasFlag(EGameObjectState.Minimized)) itemList[i].Minimize();
                if (objectState.HasFlag(EGameObjectState.Invisible)) itemList[i].SetInvisible();
            }
        }

        private static void OnComplete(RectTransform parent, Action onComplete)
        {
            // GameCore.GetInstance().StartAfterFrames(() =>
            // {
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            //     onComplete?.Invoke();
            // }, 2);
        }

        #endregion

        #region Internal modify

        public static void Modify<T>(List<T> itemList, Action<T, int> configureItem, Action onComplete = null) where T : Component
        {
            var index = 0;
            foreach (var item in itemList)
            {
                try
                {
                    if (!item.IsActive()) continue;
                    configureItem.Invoke(item, index);
                    index++;
                }
                catch (Exception ex)
                {
                    // GameLogger.LogError($"Error on modify with exception:\n{ex.Message}");
                    break;
                }
            }

            OnComplete(itemList[0].transform.parent as RectTransform, onComplete);
        }

        public static async Task ModifyAsync<T>(List<T> itemList, Func<T, int, Task> configureItem, Action onComplete = null) where T : Component
        {
            if (configureItem == null)
            {
                onComplete?.Invoke();
                return;
            }

            var tasks = itemList.Where(item => item.IsActive()).Select(configureItem).ToList();

            await Task.WhenAll(tasks);
            await Task.Yield();

            OnComplete(itemList[0].transform.parent as RectTransform, onComplete);
        }

        #endregion
    }

    public class ItemListBuilder<T> where T : Component
    {
        private readonly List<T> _itemList;
        private int _desiredCount;
        private Action<T, int> _syncConfigureItem;
        private Func<T, int, Task> _asyncConfigureItem;
        private Action _onComplete;
        private Action<T, int> _onCompleteConfigureItem;
        private EGameObjectState _objectState = EGameObjectState.Hidden;
        private bool _useAsync;

        public ItemListBuilder(List<T> itemList)
        {
            _itemList = itemList;
        }

        public ItemListBuilder<T> WithTask()
        {
            _useAsync = true;
            return this;
        }

        public ItemListBuilder<T> WithCount(int count)
        {
            _desiredCount = count;
            return this;
        }

        public ItemListBuilder<T> WithConfiguration(Action<T, int> configureAction)
        {
            _syncConfigureItem = configureAction;
            _asyncConfigureItem = (item, index) =>
            {
                configureAction(item, index);
                return Task.CompletedTask;
            };
            return this;
        }

        public ItemListBuilder<T> WithState(EGameObjectState objectState)
        {
            _objectState = objectState;
            return this;
        }

        public ItemListBuilder<T> OnComplete(Action onCompleteAction)
        {
            _onComplete = onCompleteAction;
            return this;
        }
        
        public ItemListBuilder<T> OnCompleteConfiguration(Action<T, int> configureAction)
        {
            _syncConfigureItem = configureAction;
            _asyncConfigureItem = (item, index) =>
            {
                configureAction(item, index);
                return Task.CompletedTask;
            };
            return this;
        }

        public void Adjust()
        {
            if (_useAsync)
            {
                _ = ItemListHelper.AdjustAsync(_itemList, _desiredCount, _asyncConfigureItem, _onComplete, _objectState);
            }
            else
            {
                ItemListHelper.Adjust(_itemList, _desiredCount, _syncConfigureItem, _onComplete, _onCompleteConfigureItem, _objectState);
            }
        }

        public void Modify()
        {
            if (_useAsync)
            {
                _ = ItemListHelper.ModifyAsync(_itemList, _asyncConfigureItem, _onComplete);
            }
            else
            {
                ItemListHelper.Modify(_itemList, _syncConfigureItem, _onComplete);
            }
        }
    }
}