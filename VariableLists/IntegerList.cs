﻿// ***********************************************************************
// Assembly         : BlazesCSharpSharedCode
// Author           : BlazesRus
// Created          : 07-18-2018
//
// Last Modified By : BlazesRus
// Last Modified On : 07-19-2018
// ***********************************************************************
// <copyright file="IntegerList.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CSharpSharedCode.VariableConversionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSharedCode.VariableLists
{
	/// <summary>
	/// Class IntegerList.
	/// Implements the <see cref="System.Collections.Generic.List{System.Int32}" />
	/// </summary>
	/// <seealso cref="System.Collections.Generic.List{System.Int32}" />
	public class IntegerList : List<int>
    {
		//************************************
		// Method:    AddData
		// FullName:  IntegerList::AddData
		// Access:    public
		// Returns:   int
		// Qualifier:
		//************************************
		/// <summary>
		/// Adds the data.
		/// </summary>
		/// <returns>System.Int32.</returns>
		public int AddData()
        {
            int Index = Count;
            Add(0);
            return Index;
        }

		////************************************
		//// Method:    SaveDataToFile
		//// FullName:  IntegerList::SaveDataToFile
		//// Access:    public
		//// Returns:   void
		//// Qualifier:
		//// Parameter: std::string Path
		////************************************
		//public void SaveDataToFile(string Path)
		//{
		//	string LineString;
		//	fstream CraftedIniFile;
		//	CraftedIniFile.open(Path, ios::out | ios::trunc);
		//	int DataSize = Count;
		//	if (CraftedIniFile.is_open())
		//	{
		//		for (int i = 0; i < DataSize; ++i)
		//		{
		//			if (i != 0)
		//			{//Carriage Return to next line
		//				CraftedIniFile << "\r\n";
		//			}
		//			LineString = VariableConversionFunctions::XIntToStringConversion(ElementAt(i));
		//			CraftedIniFile << LineString;
		//		}
		//		CraftedIniFile.close();
		//	}
		//	else
		//	{
		//		cout << "Failed to open" << Path << "\n";
		//	}
		//}

		//************************************
		// Method:    AsStringList
		// FullName:  IntegerList::AsStringList
		// Access:    public
		// Returns:   StringList
		// Qualifier:
		//************************************
		/// <summary>
		/// Ases the string list.
		/// </summary>
		/// <returns>StringList.</returns>
		public StringList AsStringList()
        {
            StringList TempStringList = new StringList();
            string TempString;
            int TempInt = Count;
            for (int i = 0; i < TempInt; ++i)
            {
                TempString = VariableConversionFunctions.IntToStringConversion(this[i]);
                TempStringList.Add(TempString);
            }
            return TempStringList;
        }

		//************************************
		// Method:    AddEntry
		// FullName:  IntegerList::AddEntry
		// Access:    public
		// Returns:   void
		// Qualifier:
		//Adds new entry into list at index 0(Used mainly for InfiniteScopePosInt)
		//************************************
		/// <summary>
		/// Adds the entry.
		/// </summary>
		public void AddEntry()
        {
            this.Add(0);
        }

		//************************************
		// Method:    EditLastEntry
		// FullName:  IntegerList::EditLastEntry
		// Access:    public
		// Returns:   void
		// Qualifier:
		// Parameter: int TempValue
		// Edits last value in List(Used mainly for InfiniteScopePosInt)
		//************************************
		/// <summary>
		/// Edits the last entry.
		/// </summary>
		/// <param name="TempValue">The temporary value.</param>
		public void EditLastEntry(int TempValue)
        {
            int LastIndex = Count - 1;
            this[LastIndex] = TempValue;
        }

		/// <summary>
		/// Increases the last entry.
		/// </summary>
		public void IncreaseLastEntry()
        {
            int LastIndex = this.Count - 1;
            int TempValue = this[LastIndex];
            TempValue++;
            this[LastIndex] = TempValue;
        }

		//************************************
		// Generates String with format Index:0_Index:1...Index:5
		// Method:    GenerateAsString
		// FullName:  IntegerList::GenerateAsString
		// Access:    public
		// Returns:   string
		// Qualifier:
		//************************************
		/// <summary>
		/// Generates as string.
		/// </summary>
		/// <returns>System.String.</returns>
		public string GenerateAsString()
        {
            string ListString = "";
            int TempValue;
            int TempSize = this.Count;
            for (int i = 0; i < TempSize; ++i)
            {
                TempValue = this[i];
                if (i == 0)
                {
                    ListString = VariableConversionFunctions.IntToStringConversion(TempValue);
                }
                else
                {
                    ListString += "_";
                    ListString += VariableConversionFunctions.IntToStringConversion(TempValue);
                }
            }
            return ListString;
        }

		/// <summary>
		/// Converts the string to list.
		/// </summary>
		/// <param name="Content">The content.</param>
		public void ConvertStringToList(string Content)
        {
            if (Count != 0)
            {
                Clear();
            }
            int StringSize = Content.Length;
            char CurrentChar;
            var CurrentElement = new StringBuilder();
            string CurrentAsString;
            for (int Index = 0; Index < StringSize; ++Index)
            {
                CurrentChar = Content[Index];
                CurrentAsString = CurrentElement.ToString();
                if (CurrentAsString == "")
                {
                    if (CurrentChar != '\n' && CurrentChar != ' ' && CurrentChar != '\t' && CurrentChar != '	')
                    {
                        CurrentElement.Append(CurrentChar);
                    }
                }
                else
                {
                    if (CurrentChar != '\n' && CurrentChar != ' ' && CurrentChar != '\t' && CurrentChar != '	')
                    {
                        CurrentElement.Append(CurrentChar);
                    }
                    else
                    {
                        Add(VariableConversionFunctions.ReadIntFromString(CurrentAsString));
                        CurrentElement.Clear();
                    }
                }
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerList"/> class.
		/// </summary>
		/// <param name="Value">The value.</param>
		public IntegerList(string Value)
        {
            ConvertStringToList(Value);
        }

		/// <summary>
		/// Performs an explicit conversion from <see cref="System.String"/> to <see cref="IntegerList"/>.
		/// </summary>
		/// <param name="Value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator IntegerList(string Value)
        {
            return new IntegerList(Value);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerList"/> class.
		/// </summary>
		public IntegerList(){}
    }
}
