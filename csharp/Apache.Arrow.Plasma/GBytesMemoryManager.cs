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
using System.Buffers;
using System.Threading;

namespace Apache.Arrow.Plasma
{
    internal class GBytesMemoryManager : MemoryManager<byte>
    {
        private IntPtr _gBytes;
        private IntPtr _data;
        private int _length;

        public GBytesMemoryManager(IntPtr gBytes)
        {
            GObjectInterop.g_bytes_ref(gBytes);
            _gBytes = gBytes;
            UIntPtr length = default;
            _data = GObjectInterop.g_bytes_get_data(_gBytes, ref length);
            _length = (int)length.ToUInt32();
        }

        protected override void Dispose(bool disposing)
        {
            _data = IntPtr.Zero;
            _length = 0;

            IntPtr gBytes = Interlocked.Exchange(ref _gBytes, IntPtr.Zero);
            if (gBytes != IntPtr.Zero)
            {
                GObjectInterop.g_bytes_unref(gBytes);
            }
        }

        public override unsafe Span<byte> GetSpan()
        {
            return new Span<byte>(_data.ToPointer(), _length);
        }

        public override unsafe MemoryHandle Pin(int elementIndex = 0)
        {
            // NOTE: Unmanaged memory doesn't require GC pinning because by definition it's not
            // managed by the garbage collector.

            return new MemoryHandle((_data + elementIndex).ToPointer(), default, this);
        }

        public override void Unpin()
        {
            // SEE: Pin implementation
            return;
        }
    }
}
