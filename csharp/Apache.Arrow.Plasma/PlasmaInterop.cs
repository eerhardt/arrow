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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Apache.Arrow.Plasma
{
    internal static unsafe class PlasmaInterop
    {
        private const string LibName = "libplasma-glib";

        public static PlasmaClientSafeHandle gplasma_client_new(string store_socket_name, IntPtr options) =>
            gplasma_client_new(store_socket_name, options, error: null);

        [DllImport(LibName)]
        private static extern PlasmaClientSafeHandle gplasma_client_new(string store_socket_name, IntPtr options, GError** error);

        [DllImport(LibName)]
        public static extern void gplasma_client_disconnect(PlasmaClientSafeHandle handle);

        public static PlasmaObjectIDSafeHandle gplasma_object_id_new(byte* id, UIntPtr size) =>
            gplasma_object_id_new(id, size, error: null);

        [DllImport(LibName)]
        private static extern PlasmaObjectIDSafeHandle gplasma_object_id_new(byte* id, UIntPtr size, GError** error);

        public static PlasmaCreatedObjectSafeHandle gplasma_client_create(PlasmaClientSafeHandle client, PlasmaObjectIDSafeHandle id, UIntPtr size) =>
            gplasma_client_create(client, id, size, options: IntPtr.Zero, error: null);

        [DllImport(LibName)]
        private static extern PlasmaCreatedObjectSafeHandle gplasma_client_create(PlasmaClientSafeHandle client, PlasmaObjectIDSafeHandle id, UIntPtr size, IntPtr options, GError** error);

        public static PlasmaReferredObjectSafeHandle gplasma_client_refer_object(PlasmaClientSafeHandle client, PlasmaObjectIDSafeHandle id, long timeout_ms) =>
            gplasma_client_refer_object(client, id, timeout_ms, error: null);

        [DllImport(LibName)]
        private static extern PlasmaReferredObjectSafeHandle gplasma_client_refer_object(PlasmaClientSafeHandle client, PlasmaObjectIDSafeHandle id, long timeout_ms, GError** error);

        public static bool gplasma_created_object_seal(PlasmaCreatedObjectSafeHandle @object) =>
            gplasma_created_object_seal(@object, error: null) != 0;

        [DllImport(LibName)]
        private static extern int gplasma_created_object_seal(PlasmaCreatedObjectSafeHandle @object, GError** error);

    }

    internal static unsafe class GObjectInterop
    {
        private const string LibName = "libgobject-2.0";

        public static readonly IntPtr GType_Object = new IntPtr(20 << 2);

        [DllImport(LibName)]
        public static extern void g_object_get_property(SafeHandle obj, string name, ref GValue val);

        [DllImport(LibName)]
        public static extern IntPtr g_value_get_object(ref GValue val);

        [DllImport(LibName)]
        public static extern void g_value_init(ref GValue val, IntPtr type);

        [DllImport(LibName)]
        public static extern IntPtr g_bytes_ref(IntPtr gBytes);

        [DllImport(LibName)]
        public static extern void g_bytes_unref(IntPtr gBytes);

        [DllImport(LibName)]
        public static extern IntPtr g_bytes_get_data(IntPtr gBytes, ref UIntPtr size);
    }

    internal static unsafe class ArrowInterop
    {
        private const string LibName = "libarrow-glib";

        [DllImport(LibName)]
        public static extern long garrow_buffer_get_size(IntPtr buffer);

        [DllImport(LibName)]
        public static extern IntPtr garrow_buffer_get_data(IntPtr buffer);
    }

    internal sealed class PlasmaClientSafeHandle : SafeHandle
    {
        public PlasmaClientSafeHandle()
            : base(IntPtr.Zero, ownsHandle: true)
        {

        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            //TODO?
            return true;
        }
    }

    internal sealed class PlasmaCreatedObjectSafeHandle : SafeHandle
    {
        public PlasmaCreatedObjectSafeHandle()
            : base(IntPtr.Zero, ownsHandle: true)
        {

        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            //TODO?
            return true;
        }
    }

    internal sealed class PlasmaReferredObjectSafeHandle : SafeHandle
    {
        public PlasmaReferredObjectSafeHandle()
            : base(IntPtr.Zero, ownsHandle: true)
        {

        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            //TODO?
            return true;
        }
    }

    internal sealed class PlasmaObjectIDSafeHandle : SafeHandle
    {
        public PlasmaObjectIDSafeHandle()
            : base(IntPtr.Zero, ownsHandle: true)
        {

        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            //TODO?
            return true;
        }
    }

    internal unsafe struct GError
    {
#pragma warning disable 169
        uint domain;
        int code;
        char* message;
#pragma warning restore 169
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct GValue
    {
        IntPtr type;
        long pad_1;
        long pad_2;
    }

}
