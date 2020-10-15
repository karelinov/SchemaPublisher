using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{

    /// <summary>
    /// Вспомогательный интерфейс для предоставления ID объектом
    /// </summary>
    public interface IDPContractWithID
    {
        int ID { get;}
    }


    public class DPTreeNode<T>
    {
        private readonly T _value;
        private readonly List<DPTreeNode<T>> _children = new List<DPTreeNode<T>>();

        public Dictionary<object, DPTreeNode<T>> AllNodes { get; set; } // индексированный по ID значения список всех узлов

        public DPTreeNode(T value, bool addToAllNodes = true)
        {
            _value = value;

            // добавялем Value в общий список
            if (addToAllNodes)
            {
                AllNodes = new Dictionary<object, DPTreeNode<T>>();
                AddToAllNodes(this);
            }

        }
        
        private void AddToAllNodes(DPTreeNode<T> value)
        {
            object uniqueID;
            if (value is IDPContractWithID)
                uniqueID = (value as IDPContractWithID).ID;
            else
                uniqueID = AllNodes.Values.Count + 1;

            AllNodes.Add(uniqueID, value);
        }

        private void RemoveFromAllNodes(DPTreeNode<T> value)
        {
            object uniqueID;
            if (value is IDPContractWithID)
                AllNodes.Remove((value as IDPContractWithID).ID);
            else
            {
                for(int i=0; i< AllNodes.Count; i++)
                {
                    if (AllNodes[i] as object == value as object)
                    {
                        AllNodes.Remove(i);
                        break;
                    }
                }
            }
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
            var childNode = new DPTreeNode<T>(value,false) { Parent = this };
            _children.Add(childNode);
            AddToAllNodes(childNode);
            return childNode;
        }

        /// <summary>
        /// Удаление дочернего узла
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool RemoveChild(DPTreeNode<T> childNode)
        {
            foreach (var curChildNode in childNode.FlattenNodes())
            {
                RemoveFromAllNodes(curChildNode);
            }

            return _children.Remove(childNode);
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
        /// Возврат списка с данными всей иерерхии дочерних узлов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FlattenValues()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.FlattenValues()));
        }

        /// <summary>
        /// Возврат список всей иерерхии дочерних узлов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DPTreeNode<T>> FlattenNodes()
        {
            return new[] { this }.Concat(_children.SelectMany(x => x.FlattenNodes()));
        }

        /// <summary>
        /// Вставка дочернего узла в данный узел
        /// </summary>
        /// <param name="child"></param>
        public void AddChildNode(DPTreeNode<T> childNode)
        {
            foreach (var curChildNode in childNode.FlattenNodes())
            {
                AddToAllNodes(curChildNode);
            }
            childNode.AllNodes = null;


            _children.Add(childNode);
            childNode.Parent = this;
        }

    }
}

