// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDS.OrgManager.Common.Text
{
    public class AsciiTreeNode<T>
    {
        public List<AsciiTreeNode<T>> Children { get; }

        public T Value { get; }

        public AsciiTreeNode(T value, params AsciiTreeNode<T>[] children)
        {
            Value = value;
            Children = (children ?? new AsciiTreeNode<T>[0]).ToList();
        }

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
    }
}