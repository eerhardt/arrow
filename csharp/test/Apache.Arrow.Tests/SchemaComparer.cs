﻿// Licensed to the Apache Software Foundation (ASF) under one or more
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

using System.Linq;
using Xunit;

namespace Apache.Arrow.Tests
{
    public class SchemaComparer
    {
        public static bool Equals(Schema s1, Schema s2)
        {
            if (ReferenceEquals(s1, s2))
            {
                return true;
            }
            if (s2 == null || s1 == null || s1.HasMetadata != s2.HasMetadata || s1.Fields.Count != s2.Fields.Count)
            {
                return false;
            }

            if (!s1.Fields.Keys.All(k => s2.Fields.ContainsKey(k) && FieldComparer.Equals(s1.Fields[k], s2.Fields[k])) ||
                !s2.Fields.Keys.All(k => s1.Fields.ContainsKey(k) && FieldComparer.Equals(s2.Fields[k], s1.Fields[k])))
            {
                return false;
            }

            if (s1.HasMetadata && s2.HasMetadata)
            {
                return s1.Metadata.Keys.Count() == s2.Metadata.Keys.Count() &&
                       s1.Metadata.Keys.All(k => s2.Metadata.ContainsKey(k) && s1.Metadata[k] == s2.Metadata[k]) &&
                       s2.Metadata.Keys.All(k => s1.Metadata.ContainsKey(k) && s2.Metadata[k] == s1.Metadata[k]);
            }
            return true;
        }

        public static void Compare(Schema expected, Schema actual)
        {
            if (ReferenceEquals(expected, actual))
            {
                return;
            }

            Assert.Equal(expected.HasMetadata, actual.HasMetadata);
            if (expected.HasMetadata)
            {
                Assert.Equal(expected.Metadata.Keys.Count(), actual.Metadata.Keys.Count());
                Assert.True(expected.Metadata.Keys.All(k => actual.Metadata.ContainsKey(k) && expected.Metadata[k] == actual.Metadata[k]));
                Assert.True(actual.Metadata.Keys.All(k => expected.Metadata.ContainsKey(k) && actual.Metadata[k] == expected.Metadata[k]));
            }

            Assert.Equal(expected.Fields.Count, actual.Fields.Count);
            Assert.True(expected.Fields.Keys.All(k => actual.Fields.ContainsKey(k)));
            foreach (string name in expected.Fields.Keys)
            {
                FieldComparer.Compare(expected.Fields[name], actual.Fields[name]);
            }
        }
    }
}
