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
    /// <summary>
    /// The PlasmaClient is used to interface with a plasma store.
    /// </summary>
    public class PlasmaClient
    {
        private PlasmaClientSafeHandle _handle;

        public PlasmaClient(string storeSocketName)
        {
            _handle = PlasmaInterop.gplasma_client_new(storeSocketName, options: IntPtr.Zero);
        }

        public CreatedObject CreateObject(ObjectId id, long size)
        {
            return new CreatedObject(
                PlasmaInterop.gplasma_client_create(_handle, id._handle, (UIntPtr)size));
        }

        public ReferredObject ReferObject(ObjectId id, long timeout)
        {
            return new ReferredObject(
                PlasmaInterop.gplasma_client_refer_object(_handle, id._handle, timeout));
        }
    }
}
