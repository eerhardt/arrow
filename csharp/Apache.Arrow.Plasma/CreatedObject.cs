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
    public class CreatedObject
    {
        private PlasmaCreatedObjectSafeHandle _handle;

        internal CreatedObject(PlasmaCreatedObjectSafeHandle handle)
        {
            if (handle.IsInvalid) throw new ArgumentException(nameof(handle));

            _handle = handle;
        }

        public ObjectStoreData GetData()
        {
            GValue value = default;
            GObjectInterop.g_value_init(ref value, GObjectInterop.GType_Object);

            GObjectInterop.g_object_get_property(_handle, "data", ref value);

            IntPtr buffer = GObjectInterop.g_value_get_object(ref value);

            IntPtr gBytes = ArrowInterop.garrow_buffer_get_data(buffer);
            return new ObjectStoreData(new GBytesMemoryManager(gBytes));
        }

        public void Seal()
        {
            PlasmaInterop.gplasma_created_object_seal(_handle);
        }
    }
}
