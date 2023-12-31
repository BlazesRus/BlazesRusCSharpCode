﻿/*	Code Created by James Michael Armstrong (https://github.com/BlazesRus)
    Latest Code Release at https://github.com/BlazesRus/BlazesRusSharedCode
*/

using System;

//Does not need BigMath library to compile

//CSharpSharedCode.ExperimentalCode.MediumDec
namespace CSharpSharedCode.ExperimentalCode
{
    using System.ComponentModel;
    using System.Globalization;
    using static VariableConversionFunctions.VariableConversionFunctions;

    // Represent +- 4294967295.999999999 with 100% consistency of accuracy
    //(Aka SuperDec_Int32_9Decimal)
    public
#if (!BlazesSharedCode_MediumDec_AsStruct)
    sealed
#endif
    partial
#if (!BlazesSharedCode_MediumDec_AsStruct)
    class
#else
    struct
#endif
    MediumDec : IFormattable, INotifyPropertyChanged
    {
        public static MediumDec GetValueFromString(string Value)
        {
#if (BlazesSharedCode_MediumDec_AsStruct)
            MediumDec NewSelf = MediumDec.Zero;
#else
            MediumDec NewSelf = new MediumDec();
#endif
            bool IsNegative = false;
            sbyte PlaceNumber;
            //var StringLength = (byte)Value.Length;
            string WholeNumberBuffer = "";
            string DecimalBuffer = "";

            bool ReadingDecimal = false;
            int TempInt;
            int TempInt02;
            var Decimalbuilder = new System.Text.StringBuilder("");
            var WholeNumberbuilder = new System.Text.StringBuilder("");
            foreach (char StringChar in Value)
            {
                if (IsDigit(StringChar))
                {
                    if (ReadingDecimal)
                    {
                        Decimalbuilder.Append(StringChar);
                    }
                    else
                    {
                        WholeNumberbuilder.Append(StringChar);
                    }
                }
                else if (StringChar == '-')
                {
                    IsNegative = true;
                }
                else if (StringChar == '.')
                {
                    ReadingDecimal = true;
                }
            }
            WholeNumberBuffer = WholeNumberbuilder.ToString();
            DecimalBuffer = Decimalbuilder.ToString();
            PlaceNumber = (sbyte)(WholeNumberBuffer.Length - 1);
            foreach (char StringChar in WholeNumberBuffer)
            {
                TempInt = CharAsInt(StringChar);
                TempInt02 = (TempInt * SuperDecSharedCode.PowerOfTens[PlaceNumber]);
                if (StringChar != '0')
                {
                    NewSelf.IntValue += (ushort)TempInt02;
                }
                PlaceNumber--;
            }
            PlaceNumber = 8;
            foreach (char StringChar in DecimalBuffer)
            {
                //Limit stored decimal numbers to the amount it can store
                if (PlaceNumber > -1)
                {
                    TempInt = CharAsInt(StringChar);
                    TempInt02 = (TempInt * SuperDecSharedCode.PowerOfTens[PlaceNumber]);
                    if (StringChar != '0')
                    {
                        NewSelf.DecimalStatus += (int)TempInt02;
                    }
                    PlaceNumber--;
                }
            }
            return NewSelf;
        }

        /// <summary>
        /// Display string with empty decimal places removed
        /// </summary>
        /// <returns></returns>
        public string ToOptimalString()
        {
            //string Value = "";
            var builder = new System.Text.StringBuilder("");
            uint IntegerHalf = intValue;
            byte CurrentDigit;
            bool IsNegative = DecimalStatus < 0;
            if (IsNegative)
            {
                builder.Append("-");
            }

            for (sbyte Index = NumberOfPlaces(IntegerHalf); Index >= 0; --Index)
            {
                CurrentDigit = (byte)(IntegerHalf / Math.Pow(10, Index));
                IntegerHalf -= (ushort)(CurrentDigit * Math.Pow(10, Index));
                //Value += DigitAsChar(CurrentDigit);
                builder.Append(DigitAsChar(CurrentDigit));
            }
            if (DecimalStatus != 0 && DecimalStatus != NegativeWholeNumber)
            {
                uint DecimalHalf = (uint)DecimalStatus;
                //Value += ".";
                builder.Append(".");
                for (sbyte Index = 8; Index >= 0; --Index)
                {
                    if (DecimalStatus != 0)
                    {
                        CurrentDigit = (byte) (DecimalHalf / Math.Pow(10, Index));
                        DecimalHalf -= (uint) (CurrentDigit * Math.Pow(10, Index));
                        //Value += DigitAsChar(CurrentDigit);
                        builder.Append(DigitAsChar(CurrentDigit));
                    }
                }
            }
            //return Value;
            return builder.ToString();
        }

        /// <summary>
        /// Display string with empty decimal places show
        /// </summary>
        /// <returns></returns>
        public string ToFullString()
        {
            string Value = "";
            uint IntegerHalf = intValue;
            byte CurrentDigit;
            bool IsNegative = DecimalStatus < 0;
            if (IsNegative)
            {
                Value += "-";
            }
            for (sbyte Index = NumberOfPlaces(IntegerHalf); Index >= 0; Index--)
            {
                CurrentDigit = (byte)(IntegerHalf / SuperDecSharedCode.PowerOfTens[Index]);
                IntegerHalf -= (ushort)(CurrentDigit * SuperDecSharedCode.PowerOfTens[Index]);
                Value += DigitAsChar(CurrentDigit);
            }
            if (DecimalStatus != 0 && DecimalStatus != NegativeWholeNumber)
            {
                Value += ".";
                int DecimalHalf =
                DecimalStatus;
                for (sbyte Index = 8; Index >= 0; --Index)
                {
                    CurrentDigit = (byte)(DecimalHalf / SuperDecSharedCode.PowerOfTens[Index]);
                    DecimalHalf -= (CurrentDigit * SuperDecSharedCode.PowerOfTens[Index]);
                    Value += DigitAsChar(CurrentDigit);
                }
            }
            else
            {
                Value += ".000000000";
            }
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            return String.Format(provider, this.ToOptimalString());
        }

        /// <summary>
        /// Change variable to string with certain formating style
        /// </summary>
        /// <param name="FormatStyle"></param>
        /// <returns></returns>
        public string ToString(string FormatStyle)
        {
            return ToOptimalString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToOptimalString();
        }

        ///// <summary>
        ///// Change variable into string with certain formating style with culture info set
        ///// </summary>
        ///// <param name="FormatStyle"></param>
        ///// <param name="culture"></param>
        ///// <returns></returns>
        //public string ToString(string FormatStyle, CultureInfo culture)
        //{
        //    return String.Format(culture, this.ToOptimalString());//Ensure to reformat string based on culture
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string ToString(CultureInfo culture)
        {
            return String.Format(culture, this.ToOptimalString());//Ensure to reformat string based on culture
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="numberFormat"></param>
        /// <returns></returns>
        public string ToString(NumberFormatInfo numberFormat)
        {
            return String.Format(numberFormat, this.ToOptimalString());//Ensure to reformat string based on format type
        }

#region From Standard types to this type
        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(sbyte Value)
        {
            if (Value < 0) { this.decimalStatus = NegativeWholeNumber; Value *= -1; }
            this.intValue = (ushort)Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(short Value)
        {
            if (Value < 0) { this.decimalStatus = NegativeWholeNumber; Value *= -1; }
            this.intValue = (ushort)Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(int Value)
        {
            if (Value < 0) { this.decimalStatus = NegativeWholeNumber; Value *= -1; }
            //Cap value if too big on initialize
            if (Value > 65535)
            {
                Value = 65535;
            }
            this.intValue = (ushort)Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(byte Value)
        {
            this.intValue = (ushort)Value;
            this.decimalStatus = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(ushort Value)
        {
            this.intValue = Value;
            this.decimalStatus = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(uint Value)
        {
            //Cap value if too big on initialize
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            this.intValue = (ushort)Value;
            this.decimalStatus = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(ulong Value)
        {
            //Cap value if too big on initialize
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            this.intValue = (ushort)Value;
            this.decimalStatus = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(long Value)
        {
            if (Value < 0) { this.decimalStatus = NegativeWholeNumber; Value *= -1; }
            //Cap value if too big on initialize
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            this.intValue = (ushort)Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(double Value)
        {
            bool IsNegative = Value < 0;
            if (IsNegative) { Value *= -1.0; }
            ulong WholeValue = (ulong)Math.Floor(Value);
            //Cap value if too big on initialize (preventing overflow on conversion)
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            Value -= WholeValue;
            IntValue = (ushort)WholeValue;
            decimalStatus = (int)(Value * 10000000000);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(float Value)
        {
            bool IsNegative = Value < 0;
            if (IsNegative) { Value *= -1.0f; }
            ulong WholeValue = (ulong)Math.Floor(Value);
            //Cap value if too big on initialize (preventing overflow on conversion)
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            Value -= WholeValue;
            intValue = (ushort)WholeValue;
            decimalStatus = (int)(Value * 10000000000);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(decimal Value)
        {
#if (MediumDec_UseLegacyStorage)
            if (Value < 0.0M)
            {
                Value *= -1;
                DecBoolStatus = 1;
            }
            else
            {
                DecBoolStatus = 0;
            }
#else
            bool IsNegative = Value < 0;
            if (IsNegative) { Value *= -1.0m; }
#endif
            ulong WholeValue = (ulong)Math.Floor(Value);
            //Cap value if too big on initialize (preventing overflow on conversion)
            if (Value > 4294967295)
            {
                Value = 4294967295;
            }
            Value -= WholeValue;
            intValue = (ushort)WholeValue;
#if (MediumDec_UseLegacyStorage)
            decimalStatus = (ushort)(Value * 10000);
#elif (MediumDec_ReducedSize)
            decimalStatus = (short)(Value * 10000);
#else
            decimalStatus = (int)(Value * 10000000000);
#endif
#if (!MediumDec_UseLegacyStorage)
            if (IsNegative)
            {
                if (decimalStatus == 0)
                {
                    decimalStatus = NegativeWholeNumber;
                }
                else
                {
                    decimalStatus *= -1;
                }
            }
#endif
        }

        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(string Value)
        {
            intValue = 0;
            decimalStatus = 0;
#if (MediumDec_UseLegacyStorage)
            DecBoolStatus = 0;
#else
            bool IsNegative = false;
#endif
            sbyte PlaceNumber;
            //var StringLength = (byte)Value.Length;
            string WholeNumberBuffer = "";
            string DecimalBuffer = "";

            bool ReadingDecimal = false;
            int TempInt;
            int TempInt02;
            var Decimalbuilder = new System.Text.StringBuilder("");
            var WholeNumberbuilder = new System.Text.StringBuilder("");
            foreach (char StringChar in Value)
            {
                if (IsDigit(StringChar))
                {
                    if (ReadingDecimal)
                    {
                        Decimalbuilder.Append(StringChar);
                    }
                    else
                    {
                        WholeNumberbuilder.Append(StringChar);
                    }
                }
                else if (StringChar == '-')
                {
#if (MediumDec_UseLegacyStorage)
                    DecBoolStatus = 1;
#else
                    IsNegative = true;
#endif
                }
                else if (StringChar == '.')
                {
                    ReadingDecimal = true;
                }
            }
            WholeNumberBuffer = WholeNumberbuilder.ToString();
            DecimalBuffer = Decimalbuilder.ToString();
            PlaceNumber = (sbyte)(WholeNumberBuffer.Length - 1);
            foreach (char StringChar in WholeNumberBuffer)
            {
                TempInt = CharAsInt(StringChar);
                TempInt02 = (TempInt * SuperDecSharedCode.PowerOfTens[PlaceNumber]);
                if (StringChar != '0')
                {
                    intValue += (ushort)TempInt02;
                }
                PlaceNumber--;
            }
#if (MediumDec_ReducedSize || MediumDec_UseLegacyStorage)
            PlaceNumber = 3;
#else
            PlaceNumber = 8;
#endif
            int StartingDigit = (Decimalbuilder.Length - 1);
            foreach (char StringChar in DecimalBuffer)
            {
                //Limit stored decimal numbers to the amount it can store
                if (PlaceNumber > -1)
                {
                    TempInt = CharAsInt(StringChar);
                    TempInt02 = (TempInt * SuperDecSharedCode.PowerOfTens[PlaceNumber]);
                    if (StringChar != '0')
                    {
#if (MediumDec_ReducedSize || MediumDec_UseLegacyStorage)
                        DecimalStatus += (ushort)TempInt02;
#else
                        DecimalStatus += TempInt02;
#endif
                    }
                    PlaceNumber--;
                }
            }
#if (!MediumDec_UseLegacyStorage)
            if (IsNegative)
            {
                DecimalStatus *= -1;
            }
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(bool Value)
        {
#if (MediumDec_UseLegacyStorage)
            this.DecBoolStatus = 0;
#endif
            if(Value==true)
            {
                this.IntValue = 1;
            }
            else
            {
                this.IntValue = 0;
            }
            this.DecimalStatus = 0;
        }

#endregion From Standard types to this type
#if (SharedCode_EnableDependencyPropStuff)
        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="Value"></param>
        public MediumDec(DependencyProperty Value)
        {
            var NewValue = (MediumDec)Value;
            this.DecBoolStatus = NewValue.DecBoolStatus;
            this.intValue = NewValue.intValue;
            this.decimalStatus = NewValue.DecimalStatus;
        }
#endif

#region From this type to Standard types

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator decimal(MediumDec self)
        {
            decimal Value = (decimal)self.IntValue;
#if (MediumDec_UseLegacyStorage)
            Value += (decimal)(self.DecimalStatus * 0.0001);
            if (self.DecBoolStatus == 1) { Value *= -1.0M; }
#else
            if(self.DecimalStatus<0)
            {
                if(self.DecimalStatus== NegativeWholeNumber)
                {
                    Value *= -1;
                }
                else
                {
                    self.DecimalStatus *= -1;
#if (MediumDec_ReducedSize)
                    Value += (decimal)(self.DecimalStatus * 0.0001M);
#else
                    Value += (decimal)(self.DecimalStatus * 0.000000001M);
#endif
                    Value *= -1.0M;
                }
            }
            else
            {
#if (MediumDec_ReducedSize)
                Value += (decimal)(self.DecimalStatus * 0.0001M);
#else
                Value += (decimal)(self.DecimalStatus * 0.000000001M);
#endif
            }
#endif
            return Value;
        }

        /// <summary>
        /// MediumDec to double explicit conversion
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator double(MediumDec self)
        {
            double Value = (double)self.IntValue;
#if (MediumDec_UseLegacyStorage)
            Value += (double)(self.DecimalStatus * 0.0001);
            if (self.DecBoolStatus == 1) { Value *= -1.0; }
#else
            if (self.DecimalStatus < 0)
            {
                if (self.DecimalStatus == NegativeWholeNumber)
                {
                    Value *= -1;
                }
                else
                {
                    self.DecimalStatus *= -1;
#if (MediumDec_ReducedSize)
                    Value += (double)(self.DecimalStatus * 0.0001);
#else
                    Value += (double)(self.DecimalStatus * 0.000000001);
#endif
                    Value *= -1.0;
                }
            }
            else
            {
#if (MediumDec_ReducedSize)
                Value += (double)(self.DecimalStatus * 0.0001);
#else
                Value += (double)(self.DecimalStatus * 0.000000001);
#endif
            }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator float(MediumDec self)
        {
            float Value = (float)self.IntValue;
#if (MediumDec_UseLegacyStorage)
            Value += (float)(self.DecimalStatus * 0.0001f);
            if (self.DecBoolStatus == 1) { Value *= -1.0f; }
#else
            if (self.DecimalStatus < 0)
            {
                if (self.DecimalStatus == NegativeWholeNumber)
                {
                    Value *= -1;
                }
                else
                {
                    self.DecimalStatus *= -1;
#if (MediumDec_ReducedSize)
                    Value += (float)(self.DecimalStatus * 0.0001f);
#else
                    Value += (float)(self.DecimalStatus * 0.000000001f);
#endif
                    Value *= -1.0f;
                }
            }
            else
            {
#if (MediumDec_ReducedSize)
                Value += (float)(self.DecimalStatus * 0.0001f);
#else
                Value += (float)(self.DecimalStatus * 0.000000001f);
#endif
            }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator int(MediumDec self)
        {
            int Value = (int)self.intValue;
#if (MediumDec_UseLegacyStorage)
            if (self.DecBoolStatus == 1) { Value *= -1; }
#else
            if (self.DecimalStatus < 0) { Value *= -1; }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator long(MediumDec self)
        {
            long Value = self.intValue;
#if (MediumDec_UseLegacyStorage)
            if (self.DecBoolStatus == 1) { Value *= -1; }
#else
            if (self.DecimalStatus < 0) { Value *= -1; }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator uint(MediumDec self)
        {
            return self.intValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator ulong(MediumDec self)
        {
            return self.intValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator byte(MediumDec self)
        {
            byte Value = (byte)self.intValue;
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator sbyte(MediumDec self)
        {
            sbyte Value = (sbyte)self.intValue;
#if (MediumDec_UseLegacyStorage)
            if (self.DecBoolStatus == 1) { Value *= -1; }
#else
            if (self.DecimalStatus < 0) { Value *= -1; }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator ushort(MediumDec self)
        {
            ushort Value = (ushort)self.intValue;
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator short(MediumDec self)
        {
            short Value = (short)self.intValue;
#if (MediumDec_UseLegacyStorage)
            if (self.DecBoolStatus == 1) { Value *= -1; }
#else
            if (self.DecimalStatus < 0) { Value *= -1; }
#endif
            return Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        static public explicit operator string(MediumDec self) => self.ToOptimalString();

        /// <summary>
        ///
        /// </summary>
        /// <param name="self"></param>
        public static explicit operator bool(MediumDec self)
        {
            return (int)self == 1;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="self"></param>
        //static public explicit operator dynamic(MediumDec self) => MediumDec.Initialize(self);

#endregion From this type to Standard types

        //From Standard types to this type
#if (!BlazesSharedCode_ImplicitConversionFrom)

        public static explicit operator MediumDec(decimal Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(double Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(int Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(uint Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(long Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(ulong Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(ushort Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(short Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(sbyte Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(byte Value)
        {
            return new MediumDec(Value);
        }

        public static explicit operator MediumDec(string Value)
        {
            return new MediumDec(Value);
        }

#if (SharedCode_EnableDependencyPropStuff)
        public static explicit operator MediumDec(System.Windows.DependencyProperty Value)
        {
            MediumDec NewValue = (MediumDec)Value.ToString();
            return NewValue;
        }
#endif
#else
        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(decimal Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(double Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(float Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(int Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(uint Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(long Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(ulong Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(ushort Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(short Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(sbyte Value) { return new MediumDec(Value); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(byte Value) { return new MediumDec(Value); }

        /// <summary>
        /// String converted to MediumDec
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator MediumDec(string Value) { return new MediumDec(Value); }

#if (SharedCode_EnableDependencyPropStuff)
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="Value"></param>
        //public static implicit operator MediumDec(DependencyProperty Value)
        //{
        //    //Type PropertyType = Value.PropertyType;
        //    MediumDec NewValue = Value.ToString();
        //    return NewValue;
        //}
#endif
#endif
    }
}
