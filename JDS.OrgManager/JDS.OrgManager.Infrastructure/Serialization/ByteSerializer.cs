// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Serialization
{
    public class ByteSerializer : IByteSerializer
    {
        public T Deserialize<T>(byte[] bytes) => JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(bytes));

        public byte[] Serialize<T>(T obj) => Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj));
    }
}