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
    public class ObjectId
    {
        internal PlasmaObjectIDSafeHandle _handle;

        private ObjectId(PlasmaObjectIDSafeHandle handle)
        {
            if (handle.IsInvalid) throw new ArgumentException(nameof(handle));

            _handle = handle;
        }

        public static ObjectId Create(string hexString)
        {
            return Create(StringToByteArray(hexString));
        }

        private static byte[] StringToByteArray(string hex)
        {
            int charCount = hex.Length;
            byte[] bytes = new byte[charCount / 2];
            for (int i = 0; i < charCount; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static ObjectId Create(ReadOnlySpan<byte> bytes)
        {
            unsafe
            {
                fixed (byte* pBytes = bytes)
                {
                    return new ObjectId(
                        PlasmaInterop.gplasma_object_id_new(pBytes, (UIntPtr)bytes.Length));
                }
            }
        }
    }
}
