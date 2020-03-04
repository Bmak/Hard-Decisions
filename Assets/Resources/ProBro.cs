using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class ProBro {

#region Thanks Asher Vollmer!

	// VoHelp - RANDOM   ==================================================================

	public static bool CoinFlip ()
	{
		return UnityEngine.Random.value < 0.5f;
	}

	public static int Range ( IntRange r )
	{
		return ( int )UnityEngine.Random.Range( r.min, r.max );
	}
	
	public static float Range ( FloatRange r )
	{
		return ( float )UnityEngine.Random.Range( r.min, r.max );
	}

	public static string RandomLetter ( bool spaces = false )
	{
		string val = "";
		int i = UnityEngine.Random.Range( 0, 26 + ( spaces ? 5 : 0 ) );
		switch ( i ) {
			case 0 : 
				val = "A";
				break;
			case 1 : 
				val = "B";
				break;			
			case 2 : 
				val = "C";
				break;
			case 3 : 
				val = "D";
				break;
			case 4 : 
				val = "E";
				break;
			case 5 : 
				val = "F";
				break;
			case 6 : 
				val = "G";
				break;
			case 7 : 
				val = "H";
				break;
			case 8 : 
				val = "I";
				break;
			case 9 : 
				val = "J";
				break;
			case 10 : 
				val = "K";
				break;
			case 11 : 
				val = "L";
				break;
			case 12 : 
				val = "M";
				break;
			case 13 : 
				val = "N";
				break;
			case 14 : 
				val = "O";
				break;
			case 15 : 
				val = "P";
				break;
			case 16 : 
				val = "Q";
				break;
			case 17 : 
				val = "R";
				break;
			case 18 : 
				val = "S";
				break;
			case 19 : 
				val = "T";
				break;
			case 20 : 
				val = "U";
				break;
			case 21 : 
				val = "V";
				break;
			case 22 : 
				val = "W";
				break;
			case 23 : 
				val = "X";
				break;
			case 24 : 
				val = "Y";
				break;
			case 25 : 
				val = "Z";
				break;
			default: 
				val = " ";
				break;
		}
		return val;
	}
	
	public static string RandomString ( int length )
	{
		string result = "";
		for ( int i = 0; i < length; i++ ) result += ProBro.RandomLetter( true );
		return result;
	}
	
	public static Color RandomColor ()
	{
		Color result = new Color();
		result.a = 1.0f;

		float total = 2.0f;
		result.r = UnityEngine.Random.Range( 0.0f, 1.0f );
		total -= result.r;
		result.g = Mathf.Min( UnityEngine.Random.Range( 0.0f, total ), 1.0f );
		total -= result.g;
		result.b = total;
		
		return result;
	}

	public static T PickRandom< T > ( T[] source )
	{
		return source[ UnityEngine.Random.Range( 0, source.Length ) ];
	}


	// VoHelp - TRANSFORMATION   ==========================================================

	public static string Capitalize( string s )
	{
		if ( string.IsNullOrEmpty( s ) ) return string.Empty;
		return char.ToUpper( s[ 0 ] ) + s.Substring( 1 );
	}

	public static string Yell (string str)
	{
		return str.ToUpper().Replace( " ", "\n" );
	}

	public static string ReverseString( string s )
	{
		char[] charArray = s.ToCharArray();
		System.Array.Reverse( charArray );
		return new string( charArray );
	}


	// VoHelp - UTILITIES   ===============================================================

	// NUMBERS   --------------------------------------------------------------------------
	public static float Sign ( float value )
	{
		if ( value == 0.0f ) return 0.0f;
		return Mathf.Sign( value );
	}

	public static bool Close( float f1, float f2, float epsilon = 0.0000001192093f )
	{
		return Mathf.Abs( f1 - f2 ) <= epsilon;
	}

	// TIME   -----------------------------------------------------------------------------
	public static string ReadableTimespan ( TimeSpan t )
	{
		int days = ( int )t.TotalDays;
		if ( days > 365 ) return ">1 year";
		else if ( days > 0 ) return days + "d";

		int hours = ( int )t.TotalHours;
		if ( hours > 0 ) return hours + "h";

		int minutes = ( int )t.TotalMinutes;
		if ( minutes > 0 ) return minutes + "m";

		int seconds = ( int )t.TotalSeconds;
		return seconds + "s";
	}

	public static string GetTimestamp () { return GetTimestamp( DateTime.Now ); }
	public static string GetTimestamp ( DateTime dateTime ) { return dateTime.ToString( "yyyyMMddHHmmssfff" ); }
	public static DateTime ParseTimestamp ( string timeStamp ) { return DateTime.ParseExact( timeStamp, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture ); }


	// COLOR   ----------------------------------------------------------------------------
	public static Color SetAlpha ( Color c, float a ) 
	{ 
		return new Color( c.r, c.g, c.b, a ); 
	}

	// LISTS   ----------------------------------------------------------------------------
	public static T[] ShuffleArray< T > ( T[] list ) 
	{
		int i = list.Length;
		if ( i == 0 ) return list;

		while ( i > 0 ) {
			i--;
			int j = ( int )Mathf.Floor( UnityEngine.Random.value * ( i + 1 ) );
			T tempi = list[ i ];
			T tempj = list[ j ];
			list[ i ] = tempj;
			list[ j ] = tempi;
		}

		return list;
	}

    public static List<T> ShuffleList<T>(List<T> list)
    {
        int i = list.Count;
        if (i == 0) return list;

        while (i > 0)
        {
            i--;
            int j = (int)Mathf.Floor(UnityEngine.Random.value * (i + 1));
            T tempi = list[i];
            T tempj = list[j];
            list[i] = tempj;
            list[j] = tempi;
        }

        return list;
    }

    #endregion


    public static string ArrayToString<T> (T[] list ) where T : System.IFormattable
	{
		string breaker = ", ";
		string output = "";

		for (int i = 0; i < list.Length; i++) {
			output += list[i].ToString ();

			if (i < list.Length - 1) {
				output += breaker;
			}
		}

		return output;
	}

	public static string ArrayToString<T> (T[,] array ) where T : System.IFormattable
	{
		string breaker = ", ";
		string endLine = "\n";
		string output = "";

		for (int i = 0; i < array.GetLength(0); i++) {
			for (int j = 0; j < array.GetLength (1); j++) {

				output += array[i,j].ToString ();

				if (j < array.GetLength (1) - 1) {
					output += breaker;
				}
			
			}

			if (i < array.GetLength(0) - 1) {
				output += endLine;
			}

		}

		return output;
	}

	public static string ArrayToString ( string[] list ) {
		string breaker = ", ";
		string output = "";

		for (int i = 0; i < list.Length; i++) {
			output += list[i];

			if (i < list.Length - 1) {
				output += breaker;
			}
		}

		return output;
	}

	public static string ArrayToString ( string[,] array ) {
		string breaker = ", ";
		string endLine = "\n";
		string output = "";

		for (int i = 0; i < array.GetLength(0); i++) {
			for (int j = 0; j < array.GetLength (1); j++) {

				output += array[i,j];

				if (j < array.GetLength (1) - 1) {
					output += breaker;
				}

			}

			if (i < array.GetLength(0) - 1) {
				output += endLine;
			}

		}

		return output;
	}

}

// ##################################################################################

public static class ProBroExtensions {

	// a method to set the "interactable" field TRUE - for use with callbacks
	public static void SetInteractable (this Button b) {
		b.interactable = true;
	}
	
	// a method to set the "interactable" field FALSE - for use with callbacks
	public static void SetNonInteractable (this Button b) {
		b.interactable = false;
	}

    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }

}



[ System.Serializable ]
public class FloatRange {
	public FloatRange( float mi, float ma ) { min = mi; max = ma; }
	public float min;
	public float max;
	
	public float length
	{
		get { return max - min; }
	}
}

[ System.Serializable ]
public class IntRange {
	public IntRange( int mi, int ma ) { min = mi; max = ma; }
	public int min;
	public int max;
	
	public int length
	{
		get { return max - min; }
	}
}
