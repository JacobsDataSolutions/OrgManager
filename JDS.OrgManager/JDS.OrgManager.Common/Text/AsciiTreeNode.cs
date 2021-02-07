using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDS.OrgManager.Common.Text
{
    public class AsciiTreeNode<T>
    {
        #region Public Properties + Indexers

        public List<AsciiTreeNode<T>> Children { get; }

        public T Value { get; }

        #endregion

        #region Public Constructors

        public AsciiTreeNode(T value, params AsciiTreeNode<T>[] children)
        {
            Value = value;
            Children = (children ?? new AsciiTreeNode<T>[0]).ToList();
        }

        #endregion

        #region Public Methods

        public void PrintPretty(Action<string> lineCallback)
        {
            PrintPretty(lineCallback, "", true);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            PrintPretty(s => sb.Append(s));
            return sb.ToString().Trim();
        }

        #endregion

        #region Private Methods

        private void PrintPretty(Action<string> writeCallback, string indent, bool last)
        {
            writeCallback(indent);
            if (last)
            {
                writeCallback("└ ");
                indent += "  ";
            }
            else
            {
                writeCallback("├ ");
                indent += "│ ";
            }
            writeCallback($"{Value}{Environment.NewLine}");

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].PrintPretty(writeCallback, indent, i == Children.Count - 1);
            }
        }

        #endregion
    }
}