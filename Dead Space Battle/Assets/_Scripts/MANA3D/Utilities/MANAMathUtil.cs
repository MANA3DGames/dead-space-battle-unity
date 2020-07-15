// MANAUtil: by Mahmoud A.N. Abu Obaid
using UnityEngine;

namespace MANA3D.Utilities.Math
{
	public static class MathOperation
	{
        /// <summary>
        /// [deprecated]
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
		public static float getAbsDistance( float val1, float val2 )
		{
			float a = Mathf.Abs( val1 );
			float b = Mathf.Abs( val2 );
			
			if ( a > b )
				return a - b;
			else
				return b - a;
		}

        public static float distance( float val1, float val2 )
        {
            if ( val1 > val2 )
                return val1 - val2;
            else
                return val2 - val1;
        }
	}
}
