namespace ArmChair.Http
{
    using System;
    using System.Net;

    /// <summary>
    /// A name/value pair.
    /// </summary>
    public class Parameter
    {
        private string _value;
        private string _encodedValue;


        /// <summary>
        /// Constructs a parameter object.
        /// </summary>
        public Parameter()
        {
            EncodingFunc = WebUtility.UrlEncode;
            DecodingFunc = WebUtility.UrlDecode;
        }

        /// <summary>
        /// Parameter's name.
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Parameter's value.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return _value ?? (_value = DecodingFunc(_encodedValue)); ;
            }
            set
            {
                _value = value;
                _encodedValue = null; // Forces re calculation of EncodedValue using the EncodingFunc
            }
        }
        /// <summary>
        /// Parameter's value encoded using <see cref="EncodingFunc"/> function.
        /// </summary>
        public virtual string EncodedValue
        {
            get { return _encodedValue ?? (_encodedValue = EncodingFunc(_value)); }
            set
            {
                _encodedValue = value;
                _value = null; // Forces re calculation of EncodedValue using the DecodingFunc
            }
        }

        public virtual Func<string, string> EncodingFunc { get; set; }
        public virtual Func<string, string> DecodingFunc { get; set; }
    }
}