﻿/*	Code Created by James Michael Armstrong (NexusName:BlazesRus)
    Latest Code Release at https://github.com/BlazesRus/NifLibEnvironment
*/

using System;

//Requires BigMath library to compile

//CSharpSharedCode.ExperimentalCode.SmallDec
namespace CSharpSharedCode.ExperimentalCode
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using static VariableConversionFunctions.VariableConversionFunctions;

    public partial struct LargeSuperDec : IComparable<LargeSuperDec>
    {
#pragma warning disable CC0001 // You should use 'var' whenever possible.
#pragma warning disable CC0105 // You should use 'var' whenever possible.
#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
#pragma warning disable CC0003 // Your catch should include an Exception
#pragma warning disable CS0436 // Type conflicts with imported type
#if (!BlazesSharedCode_Disable128BitFeatures)
        public static ulong ForceConvertFromInt256(BigMath.Int256 Value)
        {
            ulong ConvertedValue = 0;
            //Larger than ulong (default to zero)
            if (Value > 18446744073709551615)
            {
                Console.WriteLine("Overflow Detected");
            }
            else
            {
                BigMath.Int128 Value02 = (BigMath.Int128)Value;
                ConvertedValue = (ulong)Value02;
            }
            return ConvertedValue;
        }

        public static LargeSuperDec operator -(LargeSuperDec self, double y)
        {
            bool IsYNegative = (y < 0) ? true : false;
            y = Math.Abs(y);
            uint WholeHalfOfY = (uint)Math.Floor(y);
            y -= WholeHalfOfY;
            if (WholeHalfOfY == 0) { }
            //ex. -9 - 9
            else if (self.DecBoolStatus == 1 && IsYNegative == false)
            {// -X - Y
                self.IntValue = self.IntValue + WholeHalfOfY;
            }//ex. 9 - (-1)
            else if (self.DecBoolStatus == 0 && IsYNegative)
            {
                //X - (-Y)
                self.IntValue = self.IntValue + WholeHalfOfY;
            }
            else
            {
                // X - (Y)
                if (self.DecBoolStatus == 0)
                {
                    // ex. 8 - 9
                    if (WholeHalfOfY > self.IntValue)
                    {
                        self.IntValue = WholeHalfOfY - self.IntValue;
                        self.DecBoolStatus = 1;
                    } //ex. 8 - 7
                    else
                    {
                        self.IntValue = self.IntValue - WholeHalfOfY;
                    }
                }// -X - (Y)
                else
                {
                    // ex. -8 - (-9)
                    if (self.IntValue > WholeHalfOfY)
                    {
                        self.IntValue = WholeHalfOfY - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 - (-5)
                        self.IntValue = self.IntValue - WholeHalfOfY;
                    }
                }
            }
            //Decimal Calculation Section
            ulong SecondDec = (ulong)(System.Math.Abs(y) - System.Math.Abs(WholeHalfOfY)) * 10000000000000000000;
            if (self.DecimalStatus != 0 || SecondDec != 0)
            {
                // ex. -0.5 - 0.6
                if (self.DecBoolStatus == 1 && IsYNegative == false)
                {
                    //Potential Overflow check
                    BigMath.Int128 DecimalStatusTemp = self.DecimalStatus + SecondDec;
                    if (DecimalStatusTemp > 999999999999999999)
                    {

                        DecimalStatusTemp -= 1000000000000000000;
                        self.IntValue += 1;
                    }
                    self.DecimalStatus = (ulong)DecimalStatusTemp;
                }// ex. 0.5 - (-0.6)
                else if (self.DecBoolStatus == 0 && IsYNegative)
                {
                    //Potential Overflow check
                    BigMath.Int128 DecimalStatusTemp = self.DecimalStatus + SecondDec;
                    if (DecimalStatusTemp > 999999999999999999)
                    {
                        DecimalStatusTemp -= 1000000000000000000;
                        self.IntValue -= 1;
                    }
                    self.DecimalStatus = (ulong)DecimalStatusTemp;
                }
                else
                {
                    if (IsYNegative)
                    {// ex. -0.7 - (-0.6)
                        if (self.DecimalStatus >= SecondDec)
                        {
                            self.DecimalStatus = self.DecimalStatus - SecondDec;
                        }
                        else
                        {
                            self.DecimalStatus = SecondDec - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 0;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                    else
                    { //ex  0.6 - 0.5
                        if (self.DecimalStatus >= SecondDec)
                        {
                            self.DecimalStatus = self.DecimalStatus - SecondDec;
                        }
                        else
                        {
                            self.DecimalStatus = SecondDec - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 1;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0) { self.DecBoolStatus = 0; }
            return self;
        }

        public static LargeSuperDec operator -(LargeSuperDec self, dynamic y)
        {
            //ex. -9 - 9
            if (self.DecBoolStatus == 1 && y >= 0)
            {// -X - Y
                self.IntValue = self.IntValue + (uint)y;
            }//ex. 9 - (-1)
            else if (self.DecBoolStatus == 0 && y < 0)
            {
                //X - (-Y)
                self.IntValue = self.IntValue + (uint)Math.Abs(y);
            }
            else
            {
                // X - (Y)
                if (self.DecBoolStatus == 0)
                {
                    // ex. 8 - 9
                    if (y > self.IntValue)
                    {
                        self.IntValue = (uint)y - self.IntValue;
                        self.DecBoolStatus = 1;
                    } //ex. 8 - 7
                    else
                    {
                        self.IntValue = self.IntValue - (uint)y;
                    }
                }// -X - (Y)
                else
                {
                    uint TempY = (uint)Math.Abs(y);
                    // ex. -8 - (-9)
                    if (self.IntValue > TempY)
                    {
                        self.IntValue = TempY - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 - (-5)
                        self.IntValue = self.IntValue - TempY;
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0)
            {
                self.DecBoolStatus = 0;
            }
            return self;
        }

        public static LargeSuperDec operator %(LargeSuperDec self, int y)
        {
            if (y == 0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y < 0) { self.SwapNegativeStatus(); y *= -1; }
                if (self.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue %= (uint)y;
                }
                else
                {
                    BigMath.Int128 SelfAsInt128 = (BigMath.Int128)self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 %= y;
                    BigMath.Int128 TempStorage = SelfAsInt128 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt128 -= TempStorage;
                    self.DecimalStatus = (ulong)SelfAsInt128;
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }

            return self;
        }

        public static LargeSuperDec operator %(LargeSuperDec self, double y)
        {
            if (y == 0.0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y < 0.0) { self.SwapNegativeStatus(); y *= -1.0; }
                uint WholeHalf = (uint)y;
                //Use x Int Operation instead if y has no decimal places
                if (WholeHalf == y)
                {
                    if (self.DecimalStatus == 0)
                    {
                        //Use normal simple (int value) * (int value) if not dealing with any decimals
                        self.IntValue %= (uint)y;
                    }
                    else
                    {
                        BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                        SelfAsInt128 += self.IntValue * 10000000000000000000;
                        SelfAsInt128 *= WholeHalf;
                        self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                        SelfAsInt128 -= self.IntValue * 10000000000000000000;
                        self.DecimalStatus = (uint)SelfAsInt128;
                    }
                }
                else
                {
                    y -= WholeHalf;
                    ulong Decimalhalf;
                    if (y == 0.25)
                    {
                        Decimalhalf = 2500000000000000000;
                    }
                    else if (y == 0.5)
                    {
                        Decimalhalf = 5000000000000000000;
                    }
                    else
                    {
                        Decimalhalf = ExtractDecimalHalf(y);
                    }
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = WholeHalf;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += Decimalhalf;
                    SelfAsInt256 *= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }

            return self;
        }

        public static LargeSuperDec operator %(LargeSuperDec self, LargeSuperDec y)
        {
            if (y.IntValue == 0 && y.DecimalStatus == 0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y.DecBoolStatus == 1) { self.SwapNegativeStatus(); }
                if (self.DecimalStatus == 0 && y.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue %= y.IntValue;
                }
                else if (y.DecimalStatus == 0)
                {
                    BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 %= y.IntValue;
                    self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                    SelfAsInt128 -= self.IntValue * 10000000000000000000;
                    self.DecimalStatus = (uint)SelfAsInt128;
                }
                else
                {
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = y.IntValue;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += y.DecimalStatus;
                    SelfAsInt256 %= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }
            return self;
        }

        public static LargeSuperDec operator *(LargeSuperDec self, LargeSuperDec y)
        {
            if (y.IntValue == 0 && y.DecimalStatus == 0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y.DecBoolStatus == 1) { self.SwapNegativeStatus(); }
                if (self.DecimalStatus == 0 && y.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue *= y.IntValue;
                }
                else if (y.DecimalStatus == 0)
                {
                    BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 *= y.IntValue;
                    self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                    SelfAsInt128 -= self.IntValue * 10000000000000000000;
                    self.DecimalStatus = (uint)SelfAsInt128;
                }
                else
                {
                    //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = y.IntValue;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += y.DecimalStatus;
                    SelfAsInt256 *= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }
            return self;
        }

        public static LargeSuperDec operator *(LargeSuperDec self, int y)
        {
            if (y == 0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y < 0) { self.SwapNegativeStatus(); y *= -1; }
                if (self.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue *= (uint)y;
                }
                else
                {
                    BigMath.Int128 SelfAsInt128 = (BigMath.Int128)self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 *= y;
                    BigMath.Int128 TempStorage = SelfAsInt128 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt128 -= TempStorage;
                    self.DecimalStatus = (ulong)SelfAsInt128;
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }

            return self;
        }

        public static LargeSuperDec operator *(LargeSuperDec self, double y)
        {
            if (y == 0.0)
            {
                self.IntValue = 0;
                self.DecimalStatus = 0;
                self.DecBoolStatus = 0;
            }
            else
            {
                if (y < 0.0) { self.SwapNegativeStatus(); y *= -1.0; }
                uint WholeHalf = (uint)y;
                //Use x Int Operation instead if y has no decimal places
                if (WholeHalf == y)
                {
                    if (self.DecimalStatus == 0)
                    {
                        //Use normal simple (int value) * (int value) if not dealing with any decimals
                        self.IntValue *= (uint)y;
                    }
                    else
                    {
                        BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                        SelfAsInt128 += self.IntValue * 10000000000000000000;
                        SelfAsInt128 *= WholeHalf;
                        self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                        SelfAsInt128 -= self.IntValue * 10000000000000000000;
                        self.DecimalStatus = (uint)SelfAsInt128;
                    }
                }
                else
                {
                    y -= WholeHalf;
                    ulong Decimalhalf;
                    if (y == 0.25)
                    {
                        Decimalhalf = 2500000000000000000;
                    }
                    else if (y == 0.5)
                    {
                        Decimalhalf = 5000000000000000000;
                    }
                    else
                    {
                        Decimalhalf = ExtractDecimalHalf(y);
                    }
                    //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = WholeHalf;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += Decimalhalf;
                    SelfAsInt256 *= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }

            return self;
        }

        public static LargeSuperDec operator /(LargeSuperDec self, LargeSuperDec y)
        {
            if (y.IntValue == 0 && y.DecimalStatus == 0)
            {
                Console.WriteLine("Prevented dividing by zero");
            }
            else
            {
                if (y.DecBoolStatus == 1) { self.SwapNegativeStatus(); }
                if (self.DecimalStatus == 0 && y.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue /= y.IntValue;
                }
                else if (y.DecimalStatus == 0)
                {
                    BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 /= y.IntValue;
                    self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                    SelfAsInt128 -= self.IntValue * 10000000000000000000;
                    self.DecimalStatus = (uint)SelfAsInt128;
                }
                else
                {
                    //((self.IntValue * 10000000000000000000)+self.DecimalStatus)/(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = y.IntValue;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += y.DecimalStatus;
                    SelfAsInt256 /= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }
            return self;
        }

        public static LargeSuperDec operator /(LargeSuperDec self, int y)
        {
            if (y == 0)
            {
                Console.WriteLine("Prevented dividing by zero");
            }
            else
            {
                if (y < 0) { self.SwapNegativeStatus(); y *= -1; }
                if (self.DecimalStatus == 0)
                {//Use normal simple (int value) * (int value) if not dealing with any decimals
                    self.IntValue *= (uint)y;
                }
                else
                {
                    BigMath.Int128 SelfAsInt128 = (BigMath.Int128)self.DecimalStatus;
                    SelfAsInt128 += self.IntValue * 10000000000000000000;
                    SelfAsInt128 /= y;
                    BigMath.Int128 TempStorage = SelfAsInt128 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt128 -= TempStorage;
                    self.DecimalStatus = (ulong)SelfAsInt128;
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }
            return self;
        }

        public static LargeSuperDec operator /(LargeSuperDec self, double y)
        {
            if (y == 0)
            {
                Console.WriteLine("Prevented dividing by zero");
            }
            else
            {
                if (y < 0.0) { self.SwapNegativeStatus(); y *= -1.0; }
                uint WholeHalf = (uint)y;
                //Use x Int Operation instead if y has no decimal places
                if (WholeHalf == y)
                {
                    if (self.DecimalStatus == 0)
                    {
                        //Use normal simple (int value) * (int value) if not dealing with any decimals
                        self.IntValue /= WholeHalf;
                    }
                    else
                    {
                        BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                        SelfAsInt128 += self.IntValue * 10000000000000000000;
                        SelfAsInt128 /= WholeHalf;
                        self.IntValue = (uint)(SelfAsInt128 / 10000000000000000000);
                        SelfAsInt128 -= self.IntValue * 10000000000000000000;
                        self.DecimalStatus = (uint)SelfAsInt128;
                    }
                }
                else
                {
                    y -= WholeHalf;
                    ulong Decimalhalf;
                    if (y == 0.25)
                    {
                        Decimalhalf = 2500000000000000000;
                    }
                    else if (y == 0.5)
                    {
                        Decimalhalf = 5000000000000000000;
                    }
                    else
                    {
                        Decimalhalf = ExtractDecimalHalf(y);
                    }
                    //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                    BigMath.Int256 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int256 YAsInt256 = WholeHalf;
                    YAsInt256 *= 10000000000000000000;
                    YAsInt256 += Decimalhalf;
                    SelfAsInt256 /= YAsInt256;
                    SelfAsInt256 /= 10000000000000000000;
                    BigMath.Int256 TempStorage = SelfAsInt256 / 10000000000000000000;
                    self.IntValue = (uint)TempStorage;
                    TempStorage = self.IntValue;
                    TempStorage *= 10000000000000000000;
                    SelfAsInt256 -= TempStorage;
                    self.DecimalStatus = ForceConvertFromInt256(SelfAsInt256);
                }
                //Prevent dividing/multiplying value into nothing by dividing too small (set to .0000000000000000001 instead of having value set as zero)
                if (self.IntValue == 0 && self.DecimalStatus == 0) { self.DecimalStatus = 1; }
            }
            return self;
        }

        public static LargeSuperDec operator +(LargeSuperDec self, double y)
        {
            bool IsYNegative = (y < 0) ? true : false;
            y = Math.Abs(y);
            uint WholeHalfOfY = (uint)Math.Floor(y);
            y -= WholeHalfOfY;
            if (WholeHalfOfY == 0) { }
            else if (self.DecBoolStatus == 1 && IsYNegative)
            {// -X - Y (ex. -8 + -6)
                self.IntValue = self.IntValue + WholeHalfOfY;
            }
            else if (self.DecBoolStatus == 0 && IsYNegative == false)
            {
                //X + Y (ex. 8 + 6)
                self.IntValue = self.IntValue + WholeHalfOfY;
            }
            else
            {
                // -X + Y
                if (self.DecBoolStatus == 1)
                {   //ex. -8 + 9
                    if (y > self.IntValue)
                    {
                        self.IntValue = WholeHalfOfY - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 +  4
                        self.IntValue = self.IntValue - WholeHalfOfY;
                    }
                }// X-Y
                else
                {
                    if (self.IntValue > WholeHalfOfY)
                    {//ex. 9 + -6
                        self.IntValue = self.IntValue - WholeHalfOfY;
                    }
                    else
                    {//ex. 9 + -10
                        self.IntValue = WholeHalfOfY - self.IntValue;
                        self.DecBoolStatus = 1;
                    }
                }
            }
            //Decimal Calculation Section
            if (self.DecBoolStatus != 0 || y != 0)
            {
                ulong SecondDec = (ulong)(System.Math.Abs(y) - System.Math.Abs(WholeHalfOfY)) * 10000000000000000000;
                // ?.XXXXXX + ?.YYYYYY
                if (self.DecBoolStatus == 0 && IsYNegative == false)
                {
                    //Potential Overflow check
                    BigMath.Int128 DecimalStatusTemp = self.DecimalStatus + SecondDec;
                    if (DecimalStatusTemp > 999999999999999999)
                    {
                        DecimalStatusTemp -= 999999999999999999;
                        self.IntValue += 1;
                    }
                    self.DecimalStatus = (ulong)DecimalStatusTemp;
                }
                // -?.XXXXXX - ?.YYYYYY
                else if (self.DecBoolStatus == 1 && IsYNegative == true)
                {
                    //Potential Overflow check
                    BigMath.Int128 DecimalStatusTemp = self.DecimalStatus + SecondDec;
                    if (DecimalStatusTemp > 999999999999999999)
                    {
                        DecimalStatusTemp -= 999999999999999999;
                        self.IntValue -= 1;
                    }
                    self.DecimalStatus = (ulong)DecimalStatusTemp;
                }
                else
                {
                    if (IsYNegative)
                    {
                        // ex. 0.6 + -0.5
                        if (self.DecimalStatus >= SecondDec)
                        {
                            self.DecimalStatus = self.DecimalStatus - SecondDec;
                        }// ex. 0.6 + -.7
                        else
                        {
                            self.DecimalStatus = SecondDec - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 1;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                    else
                    {
                        if (self.DecimalStatus >= SecondDec)
                        {
                            self.DecimalStatus = self.DecimalStatus - SecondDec;
                        }// ex. -1.6 + 0.7
                        else
                        {
                            self.DecimalStatus = SecondDec - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 0;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0) { self.DecBoolStatus = 0; }
            return self;
        }

        public static LargeSuperDec operator +(LargeSuperDec self, dynamic y)
        {
            if (self.DecBoolStatus == 1 && y < 0)
            {// -X - Y (ex. -8 + -6)
                self.IntValue = self.IntValue + (uint)Math.Abs(y);
            }
            else if (self.DecBoolStatus == 0 && y >= 0)
            {
                //X + Y (ex. 8 + 6)
                self.IntValue = self.IntValue + (uint)y;
            }
            else
            {
                // -X + Y
                if (self.DecBoolStatus == 1)
                {   //ex. -8 + 9
                    if (y > self.IntValue)
                    {
                        self.IntValue = (uint)y - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 +  4
                        self.IntValue = self.IntValue - (uint)y;
                    }
                }// X-Y
                else
                {
                    uint TempY = Math.Abs(y);
                    if (self.IntValue > TempY)
                    {//ex. 9 + -6
                        self.IntValue = self.IntValue - TempY;
                    }
                    else
                    {//ex. 9 + -10
                        self.IntValue = TempY - self.IntValue;
                        self.DecBoolStatus = 1;
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0) { self.DecBoolStatus = 0; }
            return self;
        }

        // Self Less than Value
        public static bool operator <(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return false; }
                else
                {
                    // Positive Self <= -Value
                    if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return false; }
                    // Negative Self <= Value
                    else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return true; }
                    else
                    {
                        BigMath.Int128 SelfAsInt256 = self.IntValue;
                        SelfAsInt256 *= 10000000000000000000;
                        SelfAsInt256 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt256 = Value.IntValue;
                        ValueAsInt256 *= 10000000000000000000;
                        ValueAsInt256 += Value.DecimalStatus;
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return SelfAsInt256 < ValueAsInt256;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(SelfAsInt256 < ValueAsInt256);
                        }
                    }
                }
            }
            else if (Value is double)
            {
                if (Value < 0.0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            if (self.DecBoolStatus == 0) { return self.IntValue < WholeHalf; }
                            else { return !(self.IntValue < WholeHalf); }
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            if (self.DecBoolStatus == 0) { return SelfAsInt128 < (WholeHalf * 10000000000000000000); }
                            else { return !(SelfAsInt128 < (WholeHalf * 10000000000000000000)); }
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        if (self.DecBoolStatus == 0) { return SelfAsInt128 < ValueAsInt128; }
                        else { return !(SelfAsInt128 < ValueAsInt128); }
                    }
                }
            }
            else if (Value is String)
            {
                //return (String)Value == (String)self;
                return false;
            }
            else
            {
                if (Value < 0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return true; }
                    else
                    {
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return self.IntValue < Value;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(self.IntValue < Value);
                        }
                    }
                }
            }
        }

        // Self Less than or equal to Value
        public static bool operator <=(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return true; }
                else
                {
                    // Positive Self <= -Value
                    if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return false; }
                    // Negative Self <= Value
                    else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return true; }
                    else
                    {
                        BigMath.Int128 SelfAsInt256 = self.IntValue;
                        SelfAsInt256 *= 10000000000000000000;
                        SelfAsInt256 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt256 = Value.IntValue;
                        ValueAsInt256 *= 10000000000000000000;
                        ValueAsInt256 += Value.DecimalStatus;
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return SelfAsInt256 <= ValueAsInt256;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(SelfAsInt256 <= ValueAsInt256);
                        }
                    }
                }
            }
            else if (Value is double)
            {
                if (Value < 0.0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            if (self.DecBoolStatus == 0) { return self.IntValue <= WholeHalf; }
                            else { return !(self.IntValue <= WholeHalf); }
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            if (self.DecBoolStatus == 0) { return SelfAsInt128 <= (WholeHalf * 10000000000000000000); }
                            else { return !(SelfAsInt128 <= (WholeHalf * 10000000000000000000)); }
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        if (self.DecBoolStatus == 0) { return SelfAsInt128 <= ValueAsInt128; }
                        else { return !(SelfAsInt128 <= ValueAsInt128); }
                    }
                }
            }
            else if (Value is String)
            {
                //return (String)Value == (String)self;
                return false;
            }
            else
            {
                if (Value < 0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return true; }
                    else
                    {
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return self.IntValue <= Value;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(self.IntValue <= Value);
                        }
                    }
                }
            }
        }

        // Self Greater than Value
        public static bool operator >(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return false; }
                else
                {
                    // Positive Self >= -Value
                    if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return true; }
                    // Negative Self >= Value
                    else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return false; }
                    else
                    {
                        //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                        BigMath.Int128 SelfAsInt256 = self.IntValue;
                        SelfAsInt256 *= 10000000000000000000;
                        SelfAsInt256 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt256 = Value.IntValue;
                        ValueAsInt256 *= 10000000000000000000;
                        ValueAsInt256 += Value.DecimalStatus;
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return SelfAsInt256 > ValueAsInt256;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(SelfAsInt256 > ValueAsInt256);
                        }
                    }
                }
            }
            else if (Value is double)
            {
                // Positive Self >= -Value
                if (Value < 0.0 && self.DecBoolStatus == 0) { return true; }
                // Negative Self >= Value
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            if (self.DecBoolStatus == 0) { return self.IntValue > WholeHalf; }
                            else { return !(self.IntValue > WholeHalf); }
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            if (self.DecBoolStatus == 0) { return SelfAsInt128 > (WholeHalf * 10000000000000000000); }
                            else { return !(SelfAsInt128 > (WholeHalf * 10000000000000000000)); }
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        if (self.DecBoolStatus == 0) { return SelfAsInt128 >= ValueAsInt128; }
                        else { return !(SelfAsInt128 > ValueAsInt128); }
                    }
                }
            }
            else if (Value is String)
            {
                //return (String)Value == (String)self;
                return false;
            }
            else
            {
                // Positive Self >= -Value
                if (Value < 0 && self.DecBoolStatus == 0) { return true; }
                // Negative Self >= Value
                else if (Value >= 0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return false; }
                    else
                    {
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return self.IntValue > Value;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(self.IntValue > Value);
                        }
                    }
                }
            }
        }

        // Self Greater than or Equal to Value
        public static bool operator >=(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return true; }
                else
                {
                    // Positive Self >= -Value
                    if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return true; }
                    // Negative Self >= Value
                    else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return false; }
                    else
                    {
                        BigMath.Int128 SelfAsInt256 = self.IntValue;
                        SelfAsInt256 *= 10000000000000000000;
                        SelfAsInt256 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt256 = Value.IntValue;
                        ValueAsInt256 *= 10000000000000000000;
                        ValueAsInt256 += Value.DecimalStatus;
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return SelfAsInt256 >= ValueAsInt256;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(SelfAsInt256 >= ValueAsInt256);
                        }
                    }
                }
            }
            else if (Value is double)
            {
                if (Value < 0.0 && self.DecBoolStatus == 0) { return true; }
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            if (self.DecBoolStatus == 0) { return self.IntValue >= WholeHalf; }
                            else { return !(self.IntValue >= WholeHalf); }
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            if (self.DecBoolStatus == 0) { return SelfAsInt128 >= (WholeHalf * 10000000000000000000); }
                            else { return !(SelfAsInt128 >= (WholeHalf * 10000000000000000000)); }
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        if (self.DecBoolStatus == 0) { return SelfAsInt128 >= ValueAsInt128; }
                        else { return !(SelfAsInt128 >= ValueAsInt128); }
                    }
                }
            }
            else if (Value is String)
            {
                //return (String)Value == (String)self;
                return false;
            }
            else
            {
                if (Value < 0 && self.DecBoolStatus == 0) { return true; }
                else if (Value >= 0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return true; }
                    else
                    {
                        //Both are either positive or negative numbers
                        if (self.DecBoolStatus == 0)
                        {
                            return self.IntValue >= Value;
                        }
                        else
                        {//Larger number = farther down into negative
                            return !(self.IntValue >= Value);
                        }
                    }
                }
            }
        }

        // Equality operator for comparing self to int type value
        public static bool operator ==(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return true; }
                else { return false; }
            }
            else if (Value is double)
            {
                if (Value < 0.0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            return self.IntValue == WholeHalf;
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            return SelfAsInt128 == (WholeHalf * 10000000000000000000);
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        return SelfAsInt128 == ValueAsInt128;
                    }
                }
            }
            else if (Value is String)
            {
                return (String)Value == (String)self;
            }
            else
            {
                if (self.DecimalStatus != 0) { return false; }
                else if (Value < 0 && self.DecBoolStatus == 0) { return false; }
                else if (Value >= 0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return true; }
                    else { return false; }
                }
            }
        }

        // Inequality operator for comparing self to multiple value types
        public static bool operator !=(LargeSuperDec self, dynamic Value)
        {
            if (Value is LargeSuperDec)
            {
                if (self.DecBoolStatus != Value.DecBoolStatus || self.IntValue != Value.IntValue || self.DecimalStatus != Value.DecimalStatus) { return true; }
                else { return false; }
            }
            else if (Value is double)
            {
                if (Value < 0.0 && self.DecBoolStatus == 0) { return true; }
                else if (Value >= 0.0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    uint WholeHalf = (uint)Value;
                    //Use x Int Operation instead if Value has no decimal places
                    if (WholeHalf == Value)
                    {
                        if (self.DecimalStatus == 0)
                        {
                            //Use normal simple (int value) * (int value) if not dealing with anValue decimals
                            return self.IntValue != WholeHalf;
                        }
                        else
                        {
                            BigMath.Int128 SelfAsInt128 = self.DecimalStatus;
                            SelfAsInt128 += self.IntValue * 10000000000000000000;
                            return SelfAsInt128 != (WholeHalf * 10000000000000000000);
                        }
                    }
                    else
                    {
                        Value -= WholeHalf;
                        ulong Decimalhalf;
                        if (Value == 0.25)
                        {
                            Decimalhalf = 2500000000000000000;
                        }
                        else if (Value == 0.5)
                        {
                            Decimalhalf = 5000000000000000000;
                        }
                        else
                        {
                            Decimalhalf = ExtractDecimalHalf(Value);
                        }
                        BigMath.Int128 SelfAsInt128 = self.IntValue;
                        SelfAsInt128 *= 10000000000000000000;
                        SelfAsInt128 += self.DecimalStatus;
                        BigMath.Int128 ValueAsInt128 = WholeHalf;
                        ValueAsInt128 *= 10000000000000000000;
                        ValueAsInt128 += Decimalhalf;
                        return SelfAsInt128 != ValueAsInt128;
                    }
                }
            }
            else if (Value is String)
            {
                return (String)Value != (String)self;
            }
            else
            {
                if (self.DecimalStatus != 0) { return true; }
                else if (Value < 0 && self.DecBoolStatus == 0) { return true; }
                else if (Value >= 0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    Value = Math.Abs(Value);
                    if (Value == self.IntValue) { return false; }
                    else { return true; }
                }
            }
        }

        // Self Less than Value
        public static bool operator <(LargeSuperDec self, LargeSuperDec Value)
        {
            if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return false; }
            else
            {
                // Positive Self <= -Value
                if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return false; }
                // Negative Self <= Value
                else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    BigMath.Int128 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int128 ValueAsInt256 = Value.IntValue;
                    ValueAsInt256 *= 10000000000000000000;
                    ValueAsInt256 += Value.DecimalStatus;
                    //Both are either positive or negative numbers
                    if (self.DecBoolStatus == 0)
                    {
                        return SelfAsInt256 < ValueAsInt256;
                    }
                    else
                    {//Larger number = farther down into negative
                        return !(SelfAsInt256 < ValueAsInt256);
                    }
                }
            }
        }

        // Self Less than or equal to Value
        public static bool operator <=(LargeSuperDec self, LargeSuperDec Value)
        {
            if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return true; }
            else
            {
                // Positive Self <= -Value
                if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return false; }
                // Negative Self <= Value
                else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return true; }
                else
                {
                    BigMath.Int128 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int128 ValueAsInt256 = Value.IntValue;
                    ValueAsInt256 *= 10000000000000000000;
                    ValueAsInt256 += Value.DecimalStatus;
                    //Both are either positive or negative numbers
                    if (self.DecBoolStatus == 0)
                    {
                        return SelfAsInt256 <= ValueAsInt256;
                    }
                    else
                    {//Larger number = farther down into negative
                        return !(SelfAsInt256 <= ValueAsInt256);
                    }
                }
            }
        }

        // Self Greater than Value
        public static bool operator >(LargeSuperDec self, LargeSuperDec Value)
        {
            if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return false; }
            else
            {
                // Positive Self >= -Value
                if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return true; }
                // Negative Self >= Value
                else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    //((self.IntValue * 10000000000000000000)+self.DecimalStatus)*(DecimalAsInt+(WholeHalf*10000000000000000000))/10000000000000000000 = ((self.IntValue*10000000000000000000)+self.DecimalStatus))
                    BigMath.Int128 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int128 ValueAsInt256 = Value.IntValue;
                    ValueAsInt256 *= 10000000000000000000;
                    ValueAsInt256 += Value.DecimalStatus;
                    //Both are either positive or negative numbers
                    if (self.DecBoolStatus == 0)
                    {
                        return SelfAsInt256 > ValueAsInt256;
                    }
                    else
                    {//Larger number = farther down into negative
                        return !(SelfAsInt256 > ValueAsInt256);
                    }
                }
            }
        }

        // Self Greater than or Equal to Value
        public static bool operator >=(LargeSuperDec self, LargeSuperDec Value)
        {
            if (self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus) { return true; }
            else
            {
                // Positive Self >= -Value
                if (Value.DecBoolStatus == 1 && self.DecBoolStatus == 0) { return true; }
                // Negative Self >= Value
                else if (Value.DecBoolStatus == 0 && self.DecBoolStatus == 1) { return false; }
                else
                {
                    BigMath.Int128 SelfAsInt256 = self.IntValue;
                    SelfAsInt256 *= 10000000000000000000;
                    SelfAsInt256 += self.DecimalStatus;
                    BigMath.Int128 ValueAsInt256 = Value.IntValue;
                    ValueAsInt256 *= 10000000000000000000;
                    ValueAsInt256 += Value.DecimalStatus;
                    //Both are either positive or negative numbers
                    if (self.DecBoolStatus == 0)
                    {
                        return SelfAsInt256 >= ValueAsInt256;
                    }
                    else
                    {//Larger number = farther down into negative
                        return !(SelfAsInt256 >= ValueAsInt256);
                    }
                }
            }
        }
#else

#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static LargeSuperDec operator +(LargeSuperDec self, LargeSuperDec y)
        {
            bool IsYNegative = (y.DecBoolStatus == 1) ? true : false;
            if (self.DecBoolStatus == 1 && IsYNegative)
            {// -X - Y (ex. -8 + -6)
                self.IntValue = self.IntValue + y.IntValue;
            }
            else if (self.DecBoolStatus == 0 && IsYNegative == false)
            {
                //X + Y (ex. 8 + 6)
                self.IntValue = self.IntValue + y.IntValue;
            }
            else
            {
                // -X + Y
                if (self.DecBoolStatus == 1)
                {   //ex. -8 + 9
                    if (y.IntValue > self.IntValue)
                    {
                        self.IntValue = y.IntValue - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 +  4
                        self.IntValue = self.IntValue - y.IntValue;
                    }
                }// X + -Y
                else
                {
                    if (self.IntValue > y.IntValue)
                    {//ex. 9 + -6
                        self.IntValue = self.IntValue - y.IntValue;
                    }
                    else
                    {//ex. 9 + -10
                        self.IntValue = y.IntValue - self.IntValue;
                        self.DecBoolStatus = 1;
                    }
                }
            }
            //Decimal Section
            if (self.DecimalStatus != 0 || y.DecimalStatus != 0)
            {
                // ?.XXXXXX + ?.YYYYYY (ex. 0.9 + 0.2)
                if (self.DecBoolStatus == 0 && IsYNegative == false)
                {
                    //Potential Overflow check
                    try
                    {
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.IntValue += 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                    catch
                    {
                        if (self.DecimalStatus >= 2000000000000000000)
                        {
                            self.DecimalStatus -= 2000000000000000000;
                        }
                        else
                        {
                            y.DecimalStatus -= 2000000000000000000;
                        }
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.DecimalStatus += 2000000000000000000;
                            self.IntValue += 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                }
                // -?.XXXXXX - ?.YYYYYY (ex. -0.9 + -0.2)
                else if (self.DecBoolStatus == 1 && IsYNegative)
                {
                    //Potential Overflow check
                    try
                    {
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.IntValue -= 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                    catch
                    {
                        if (self.DecimalStatus >= 2000000000000000000)
                        {
                            self.DecimalStatus -= 2000000000000000000;
                        }
                        else
                        {
                            y.DecimalStatus -= 2000000000000000000;
                        }
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.DecimalStatus += 2000000000000000000;
                            self.IntValue -= 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                }
                else
                {
                    if (IsYNegative)
                    {
                        // ex. 0.6 + -0.5
                        if (self.DecimalStatus >= y.DecimalStatus)
                        {
                            self.DecimalStatus = self.DecimalStatus - y.DecimalStatus;
                        }// ex. 0.6 + -.7
                        else
                        {
                            self.DecimalStatus = y.DecimalStatus - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 1;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                    else
                    { //ex -0.6 + 0.5
                        if (self.DecimalStatus >= y.DecimalStatus)
                        {
                            self.DecimalStatus = self.DecimalStatus - y.DecimalStatus;
                        }// ex. -1.6 + 0.7
                        else
                        {
                            self.DecimalStatus = y.DecimalStatus - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 0;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0) { self.DecBoolStatus = 0; }
            return self;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static LargeSuperDec operator -(LargeSuperDec self, LargeSuperDec y)
        {
            bool IsYNegative = (y.DecBoolStatus == 1) ? true : false;
            //ex. -9 - 9
            if (self.DecBoolStatus == 1 && IsYNegative == false)
            {// -X - Y
                self.IntValue = self.IntValue + y.IntValue;
            }//ex. 9 - (-1)
            else if (self.DecBoolStatus == 0 && IsYNegative == true)
            {
                //X - (-Y)
                self.IntValue = self.IntValue + y.IntValue;
            }
            else
            {
                // X - (Y)
                if (self.DecBoolStatus == 0)
                {
                    // ex. 8 - 9
                    if (y.IntValue > self.IntValue)
                    {
                        self.IntValue = y.IntValue - self.IntValue;
                        self.DecBoolStatus = 1;
                    } //ex. 8 - 7
                    else
                    {
                        self.IntValue = self.IntValue - y.IntValue;
                    }
                }// -X - (Y)
                else
                {
                    // ex. -8 - (-9)
                    if (self.IntValue > y.IntValue)
                    {
                        self.IntValue = y.IntValue - self.IntValue;
                        self.DecBoolStatus = 0;
                    }
                    else
                    {//ex. -8 - (-5)
                        self.IntValue = self.IntValue - y.IntValue;
                    }
                }
            }
            //Decimal Section
            if (self.DecimalStatus != 0 || y.DecimalStatus != 0)
            {
                //ulong SecondDec = (ulong)(System.Math.Abs(y) - System.Math.Abs(WholeHalfOfY)) * 10000000000000000000;
                // ex. -0.5 - 0.6
                if (self.DecBoolStatus == 1 && IsYNegative == false)
                {
                    //Potential Overflow check
                    try
                    {
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.IntValue += 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                    catch
                    {
                        if (self.DecimalStatus >= 2000000000000000000)
                        {
                            self.DecimalStatus -= 2000000000000000000;
                        }
                        else
                        {
                            y.DecimalStatus -= 2000000000000000000;
                        }
                        ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                        if (DecimalStatusTemp > 9999999999999999999)
                        {
                            DecimalStatusTemp -= 10000000000000000000;
                            self.DecimalStatus += 2000000000000000000;
                            self.IntValue += 1;
                        }
                        self.DecimalStatus = DecimalStatusTemp;
                    }
                }// ex. 0.5 - (-0.6)
                else if (self.DecBoolStatus == 0 && IsYNegative)
                {
                    if (self.DecBoolStatus == 1 && IsYNegative == false)
                    {
                        //Potential Overflow check
                        try
                        {
                            ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                            if (DecimalStatusTemp > 9999999999999999999)
                            {
                                DecimalStatusTemp -= 10000000000000000000;
                                self.IntValue -= 1;
                            }
                            self.DecimalStatus = DecimalStatusTemp;
                        }
                        catch
                        {
                            if (self.DecimalStatus >= 2000000000000000000)
                            {
                                self.DecimalStatus -= 2000000000000000000;
                            }
                            else
                            {
                                y.DecimalStatus -= 2000000000000000000;
                            }
                            ulong DecimalStatusTemp = self.DecimalStatus + y.DecimalStatus;
                            if (DecimalStatusTemp > 9999999999999999999)
                            {
                                DecimalStatusTemp -= 10000000000000000000;
                                self.DecimalStatus += 2000000000000000000;
                                self.IntValue -= 1;
                            }
                            self.DecimalStatus = DecimalStatusTemp;
                        }
                    }
                }
                else
                {
                    if (IsYNegative)
                    {// ex. -0.7 - (-0.6)
                        if (self.DecimalStatus >= y.DecimalStatus)
                        {
                            self.DecimalStatus = self.DecimalStatus - y.DecimalStatus;
                        }
                        else
                        {
                            self.DecimalStatus = y.DecimalStatus - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 0;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                    else
                    { //ex  0.6 - 0.5
                        if (self.DecimalStatus >= y.DecimalStatus)
                        {
                            self.DecimalStatus = self.DecimalStatus - y.DecimalStatus;
                        }
                        else
                        {
                            self.DecimalStatus = y.DecimalStatus - self.DecimalStatus;
                            if (self.IntValue == 0)
                            {
                                self.DecBoolStatus = 1;
                            }
                            else
                            {
                                self.IntValue -= 1;
                            }
                        }
                    }
                }
            }
            //Fix potential negative zero
            if (self.IntValue == 0 && self.DecBoolStatus == 1 && self.DecimalStatus == 0) { self.DecBoolStatus = 0; }
            return self;
        }
        // Equality operator for comparing self to int type value
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool operator ==(LargeSuperDec self, LargeSuperDec Value) => self.DecBoolStatus == Value.DecBoolStatus && self.IntValue == Value.IntValue && self.DecimalStatus == Value.DecimalStatus ? true : false;

        // Inequality operator for comparing self to multiple value types
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool operator !=(LargeSuperDec self, LargeSuperDec Value)
        {
            return self.DecBoolStatus != Value.DecBoolStatus || self.IntValue != Value.IntValue || self.DecimalStatus != Value.DecimalStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static LargeSuperDec operator -(LargeSuperDec Value)
        {
            if (Value.DecBoolStatus % 2==1)//ODD
            {
                Value.DecBoolStatus -= 1;
            }
            else
            {
                Value.DecBoolStatus += 1;
            }
            return Value;
        }
    }
}