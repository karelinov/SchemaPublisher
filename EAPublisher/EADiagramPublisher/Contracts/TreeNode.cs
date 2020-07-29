using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{
    public class DPTreeNode<T>
    {
        private readonly T _value;
        private readonly List<DPTreeNode<T>> _children = new List<DPTreeNode<T>>();

        public DPTreeNode(T value)
        {
            _value = value;
        }

        // ============================================================================
        //    СВОЙСТВА
        // ============================================================================

        /// <summary>
        /// Родительский узел
        /// </summary>
        public DPTreeNode<T> Parent { get; private set; }

        /// <summary>
        /// Данные узла
        /// </summary>
        public T Value { get { return _value; } }

        // Список дочерних узлов
        public ReadOnlyCollection<DPTreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        /// <summary>
        /// Индексатор дочерних узлов
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DPTreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        // ============================================================================
        //    Методы
        // ============================================================================

        /// <summary>
        /// Создание дочернего узла (с указанными данными)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DPTreeNode<T> AddChild(T value)
        {
            var node = new DPTreeNode<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        /// <summary>
        /// Создание дочерних узлов (с указанными данными)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DPTreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        /// <summary>
        /// Удаление дочернего узла
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool RemoveChild(DPTreeNode<T> node)
        {
            return _children.Remove(node);
        }

        /// <summary>
        /// Функция обработки всех узлов делегатом
        /// </summary>
        /// <param name="action"></param>
        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        /// <summary>
        /// Возврат списка с данными всех дочерних узлов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }

        /// <summary>
        /// Вставка дочернего узла в указанный узел
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        DPTreeNode<T> InsertChild(DPTreeNode<T> parent, T value)
        {
            var node = new DPTreeNode<T>(value) { Parent = parent };
            parent._children.Add(node);
            return node;
        }

        /// <summary>
        /// Вставка дочернего узла в данный узел
        /// </summary>
        /// <param name="child"></param>
        public void AddChildNode(DPTreeNode<T> child)
        {
            _children.Add(child);
            child.Parent = this;
        }

    }
}

