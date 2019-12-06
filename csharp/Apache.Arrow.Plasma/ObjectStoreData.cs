// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace Apache.Arrow.Plasma
{
    public sealed class ObjectStoreData : IDisposable
    {
        private readonly GBytesMemoryManager _memoryManager;

        internal ObjectStoreData(GBytesMemoryManager memoryManager)
        {
            _memoryManager = memoryManager;
        }

        // TODO: Should we make a ReadOnlyObjectStoreData and a mutable ObjectStoreData?
        // That way a referred object can't get its memory changed.
        public Memory<byte> Memory => _memoryManager.Memory;

        public void Dispose()
        {
            ((IDisposable)_memoryManager).Dispose();
        }
    }
}
