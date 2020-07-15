// MANAUtil: by Mahmoud A.N. Abu Obaid
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MANA3D.Utilities.Optimization
{
    public class ObjectInfo : MonoBehaviour
    {
        public ObjectPool MyPool { set; get; }

        public void recycle()
        {
            MyPool.freeObject( gameObject );
        }
    }

	public class ObjectPool
	{
		#region private global variables
		
		// start with underscore
		private List<GameObject> _objectList;	// List of objects that will be recycled.
		private GameObject _objectPrefab;		// Object that will be instantiated/recycled.
        private Transform _poolTransform;
		private Vector3 INITIAL_POS = new Vector3( 10000, 10000, 10000 );	// Initial position.
		
		
		#endregion
		
		
		#region public Funtions
		
		// ***************************************************************
		// ObjectPool: Public Constructor.
		// ***************************************************************
		public ObjectPool( GameObject prefab, int totalObjectsAtStart, string poolName )
		{
			// Init objects list.
			_objectList = new List<GameObject>( totalObjectsAtStart );
			_objectPrefab = prefab;

            GameObject go = new GameObject(poolName);
            _poolTransform = go.transform;
			
			for ( int i = 0; i < totalObjectsAtStart; i++ )
			{
				// Create a new instance.
				GameObject newObject = Object.Instantiate( prefab, INITIAL_POS, Quaternion.identity ) as GameObject;

                // Attach ObjectInfo script to this object.
                ObjectInfo info = newObject.AddComponent<ObjectInfo>();

				// Register current object pool for this object, for recycling.
				//newObject.SendMessage( "registerMyPool", this, SendMessageOptions.DontRequireReceiver );
                info.MyPool = this;


				// Deactivate the object.
				newObject.SetActive( false );
				
				// Add the new object to the pool list.
				_objectList.Add( newObject );


                // Just for organization.
                newObject.transform.parent = _poolTransform;
			}
		}

		
		
		
		// ***************************************************************
		// ObjectPool: getNextFree.
		// Gets the next available free object.
		// ***************************************************************
		public GameObject getNextFree()
		{
			// use FirstOrDefault() to return the first one or defualt which null.
			var freeObject = ( from item in _objectList
			                  where item.activeSelf == false
			                  select item ).FirstOrDefault();
			
			// Check if there is no free object left in the pool.
			if ( freeObject == null )
			{
				// Create a new object.
				freeObject = Object.Instantiate( _objectPrefab ) as GameObject;

                // Attach ObjectInfo script to this object.
                ObjectInfo info = freeObject.AddComponent<ObjectInfo>();

                // Register current object pool for this object, for recycling.
                //newObject.SendMessage( "registerMyPool", this, SendMessageOptions.DontRequireReceiver );
                info.MyPool = this;

				// Add the new object to the pool list.
				_objectList.Add( freeObject );
			}

            // Un-parent the object.
            freeObject.transform.parent = null;

			// Deactivate the object.
			freeObject.SetActive( true );
			
			// Return the free object.
			return freeObject;
		}
		
		
		// ***************************************************************
		// ObjectPool: freeObject.
		// Recycle the object for future use.
		// ***************************************************************
		public void freeObject( GameObject objectToFree )
		{
			// reset object position.
			objectToFree.transform.position = INITIAL_POS;
			
			// Deactivate the object.
			objectToFree.SetActive( false );

            // Just for organization.
            objectToFree.transform.parent = _poolTransform;
		}
		
		
		#endregion
		
	}
}