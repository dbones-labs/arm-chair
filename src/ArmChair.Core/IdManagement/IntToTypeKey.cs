// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
using System;

namespace ArmChair.IdManagement
{
    public class IntToTypeKey : Key
    {
        private readonly object _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public IntToTypeKey(Type type, int id, string couchId)
        {
            //TODO: extract int from the couchdb.

            _id = id;
            _readFriendly = string.Format("{0}/{1}", type.FullName, id);
            _hash = _readFriendly.GetHashCode();
        }

        public override object Id
        {
            get { return _id; }
        }

        public override string CouchDbId
        {
            get { return _readFriendly; }
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            return _readFriendly;
        }
    }
}