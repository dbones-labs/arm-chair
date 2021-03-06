﻿// Copyright 2014 - dbones.co.uk (David Rundle)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair.Commands
{
    using System.Collections.Generic;

    public class CreateIndexResponse
    {
        public string Result { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class CreateIndexRequest 
    {
        public CreateIndexRequest()
        {
            Index = new Index();
        }
        public string Ddoc { get; set; }
        public string Name { get; set; }
        public Index Index { get; set; }
    }

    public class Index
    {
        public Index()
        {
            Fields = new List<IDictionary<string, string>>();
        }
        public IList<IDictionary<string, string>> Fields { get; set; }
    }
}