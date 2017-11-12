// Copyright 2014 - dbones.co.uk (David Rundle)
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
namespace ArmChair.Utils
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class ExpressionExtensions
    {
        public static CallGet CreateFieldGet(this FieldInfo field)
        {
            ParameterExpression target = Expression.Parameter(typeof(object), "target");

            MemberExpression member = Expression.Field(Expression.Convert(target, field.DeclaringType), field);

            Expression<CallGet> lambda = Expression.Lambda<CallGet>(
                Expression.Convert(member, typeof(object)), target);

            return lambda.Compile();
        }


        public static CallSet CreateFieldSet(this FieldInfo field)
        {
            if (field.IsInitOnly)
            {
                return IlCreateFieldSet(field);
            }

            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            MemberExpression member = Expression.Field(Expression.Convert(target, field.DeclaringType), field);
            BinaryExpression assign = Expression.Assign(member, Expression.Convert(value, field.FieldType));

            Expression<CallSet> lambda = Expression.Lambda<CallSet>(
                Expression.Convert(assign, typeof(object)), target, value);

            return lambda.Compile();
        }

        private static CallSet IlCreateFieldSet(FieldInfo field)
        {
            //this will set a readonly field.
            DynamicMethod dmethod = new DynamicMethod(
                ShortGuid.NewGuid(), 
                typeof(void),
                new[] { typeof(object), typeof(object) }, 
                field.DeclaringType.GetTypeInfo().Module, 
                true);
            var il = dmethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); //get instance
            il.Emit(OpCodes.Ldarg_1); //get the value
            il.Emit(OpCodes.Stfld, field); //set the field
            il.Emit(OpCodes.Ret);
           
            var setField = (CallSet)dmethod.CreateDelegate(typeof(CallSet));
            return setField;
        }

    }

}
