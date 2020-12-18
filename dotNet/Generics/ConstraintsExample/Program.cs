using System;

//The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.Warning CS8632 ConstraintsExample  C:\Code\gm\pm-academy-demo\dotNet\Generics\ConstraintsExample\Program.cs    7	Active
#nullable enable

namespace ConstraintsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generics constraints");
            NonNulableValueType();
            NonNullableReferenceType();
            NullableReferenceType();
            NotNullType();
            UnmanagerType();
            BaseClassType();
            IExampleNonNullableType();
            IExampleNullableType();
            ParameterlessConstructorType();
            DeriveFromIExampleType();
            Console.ReadLine();
        }

        private static void NonNulableValueType()
        {
            var ins1 = 123;
            var ins2 = "123";
            var ins3 = new SimpleStruct(123);
            var ins4 = IntPtr.Zero;

            var m1m1 = new NonNullableValue<int>(ins1);
            //var m1m2 = new M1<string>(ins2); // compile-time error 

            var m1m3 = new NonNullableValue<SimpleStruct>(ins3);
            var m1m4 = new NonNullableValue<IntPtr>(ins4);
        }


        private static void NonNullableReferenceType()
        {
            BaseClass ins1 = new BaseClass();
            BaseClass ins2 = default;

            var m2m1 = new NonNullableReferenceValue<BaseClass>(ins1);         // BaseClass instance can not be null
            var m2m2 = new NonNullableReferenceValue<BaseClass>(ins2);         // Warning: BaseClass instance can not be null
        }

        private static void NullableReferenceType()
        {
            BaseClass? ins1 = new BaseClass();
            string? ins2 = default;

            var n2m1 = new NullableReferenceValue<BaseClass?>(ins1);        // BaseClass instance can be null
            var n2m2 = new NullableReferenceValue<string?>(ins2);           // string instance can  be null
        }

        private static void NotNullType()
        {
            int ins1 = 123;
            BaseClass ins2 = new BaseClass();
            string ins3 = string.Empty;
            BaseClass? ins4 = default;
            int? ins5 = 5;
            int? ins6 = default;

            var n3m1 = new NotNullValue<int>(123);
            var n3m2 = new NotNullValue<BaseClass>(ins2);
            var n3m3 = new NotNullValue<string>(ins3);
            var n3m4 = new NotNullValue<BaseClass?>(ins4);        // Warning: Nullability of type argument 'ConstraintsExample.BaseClass?' doesn't match 'notnull' constraint.
            var n3m5 = new NotNullValue<int?>(ins5);
            var n3m6 = new NotNullValue<int?>(ins6);
        }

        private static void UnmanagerType()
        {
            IntPtr ins1 = IntPtr.Zero;
            int ins2 = 123;

            var n4m1 = new UnmanagedValue<IntPtr>(ins1);
            var n4m2 = new UnmanagedValue<int>(ins2);
        }

        private static void BaseClassType()
        {
            var ins1 = new BaseClass();
            var ins2 = new DerivedClass();
            var ins3 = default(BaseClass);
            var ins4 = default(string);

            var m5m1 = new BaseClassValue<BaseClass>(ins1);
            var m5m2 = new BaseClassValue<DerivedClass>(ins2);
            var m5m3 = new BaseClassValue<BaseClass>(ins3);
            // Error: The type 'string' cannot be used as type parameter 'T' in the generic type or method 'M5<T>'
            //var m5m4 = new BaseClassValue<string>(ins4);
        }

        public static void IExampleNonNullableType()
        {
            var ins1 = new DerivedExample();
            var ins2 = new StructExample();
            var ins3 = new DerivedClass();

            var m6m1 = new NonNullableIExampleValue<DerivedExample>(ins1);
            var m6m2 = new NonNullableIExampleValue<StructExample>(ins2);
            var m6m3 = new NonNullableIExampleValue<IExample>(ins1);
            var m6m4 = new NonNullableIExampleValue<IExample>(ins2);
            // Error
            //var m6m4 = new NonNullableIExampleValue<DerivedClass>(ins3);
        }

        public static void IExampleNullableType()
        {
            DerivedExample ins1 = new DerivedExample();
            StructExample ins2 = new StructExample();
            DerivedExample ins3 = default(DerivedExample);

            var m6m1 = new NullableIExampleValue<DerivedExample?>(ins1);
            var m6m2 = new NullableIExampleValue<StructExample>(ins2);
            // warning
            var m6m3 = new NullableIExampleValue<IExample?>(ins3);
        }

        public static void ParameterlessConstructorType()
        {
            var ins1 = new BaseClass();
            var ins2 = new BaseClassV2("123");
            var ins3 = 213;
            BaseClass ins4 = default;
            BaseClassV2 ins5 = default;


            var m7m1 = new ParameterlessConstructorValue<BaseClass>(ins1);
            // erorr
            //var m7m2 = new ParameterlessConstructorValue<BaseClassV2>(ins2);
            var m7m3 = new ParameterlessConstructorValue<int>(ins3);
            var m7m4 = new ParameterlessConstructorValue<BaseClass>(ins4);
            //var m7m5 = new ParameterlessConstructorValue<BaseClassV2>(ins5);
        }

        private static void DeriveFromIExampleType()
        {
            var ins1 = new StructExample();
            var ins2 = new DerivedExample();
            var ins3 = new BaseClass();
            var ins4 = new AppenderExample();
            var ins5 = new AppenderExample2(122);

            var m8m1 = new DerivedTypeInMethodValue<IExample>();
            m8m1.Add(ins1);
            m8m1.Add(ins2);
            // error
            //m8m1.Add(ins3);

            var m8m2 = new DerivedTypeInValue<IAppender<IExample>, string, IExample>("123")
            {
                DerivedData = ins2,
                Appender = ins4
            };
        }
    }
}
