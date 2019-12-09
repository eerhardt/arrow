// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

/* Header for class org_apache_arrow_plasma_PlasmaClientC */

#include "plasma/client.h"

#ifndef _Included_org_apache_arrow_plasma_PlasmaClientC
#define _Included_org_apache_arrow_plasma_PlasmaClientC
#ifdef __cplusplus
extern "C" {
#endif

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_connect(
    const char* store_socket_name, plasma::PlasmaClient** client);

/*DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_disconnect(JNIEnv*,
                                                                               jclass,
                                                                               jlong);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_create(
    JNIEnv*, jclass, jlong, jbyteArray, jint, jbyteArray);

DLLEXPORT int32_t
org_apache_arrow_plasma_PlasmaClientC_hash(JNIEnv*, jclass, jlong, jbyteArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_seal(JNIEnv*, jclass,
                                                                         jlong,
                                                                         jbyteArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_release(JNIEnv*,
                                                                            jclass, jlong,
                                                                            jbyteArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_delete(JNIEnv*,
                                                                           jclass, jlong,
                                                                           jbyteArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_get(
    JNIEnv*, jclass, jlong, jobjectArray, jint);

DLLEXPORT int32_t
org_apache_arrow_plasma_PlasmaClientC_contains(JNIEnv*, jclass, jlong, jbyteArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_fetch(JNIEnv*, jclass,
                                                                          jlong,
                                                                          jobjectArray);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_wait(
    JNIEnv*, jclass, jlong, jobjectArray, jint, jint);

DLLEXPORT int32_t org_apache_arrow_plasma_PlasmaClientC_evict(JNIEnv*,
                                                                           jclass, jlong,
                                                                           jlong);
*/
#ifdef __cplusplus
}
#endif
#endif
