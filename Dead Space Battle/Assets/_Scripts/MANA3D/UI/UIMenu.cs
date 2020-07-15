using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MANA3D.UI
{
    public class UIMenu
    {
        protected GameObject root;
        public bool IsActive { get { return root.activeSelf; } }

        Dictionary<string, GameObject> components;
        public Dictionary<string, GameObject> Components { get { return components; } }


        public UIMenu( GameObject root )
        {
            this.root = root;
            this.components = new Dictionary<string, GameObject>();

            if ( root.transform.childCount > 0 )
            {
                for ( int i = 0; i < root.transform.childCount; i++ )
                {
                    components.Add( root.transform.GetChild(i).name, 
                                    root.transform.GetChild(i).gameObject );
                }
            }
        }

        public void ShowRoot( bool show )
        {
            root.SetActive( show );
        }

        public void ShowAllComponents( bool show )
        {
            foreach (var item in components)
                item.Value.SetActive( show );
        }

        public void ShowComponent( string name, bool show )
        {
            GameObject component = null;
            if ( components.TryGetValue( name, out component ) )
                component.SetActive( show );
            else
                Debug.Log( name + " couldn't be found in " + root.name );
        }


        public bool GetActive( string name )
        {
            GameObject component = null;
            if ( components.TryGetValue( name, out component ) )
                return component.activeSelf;
            
            return false;
        }

        public bool AddListenerTo( string componentName, UnityEngine.Events.UnityAction listener )
        {
            GameObject go;
            if ( components.TryGetValue( componentName, out go ) )
            {
                Button btn = go.GetComponent<Button>();
                if ( !btn )
                    btn = go.AddComponent<Button>();

                btn.onClick.AddListener( listener );
                return true;
            }

            return false;
        }

        public bool SetText( string componentName, string text, bool internalTxt = false )
        {
            GameObject go;
            if ( components.TryGetValue( componentName, out go ) )
            {
                Text txt = internalTxt ? go.GetComponentInChildren<Text>() : go.GetComponent<Text>();
                if ( txt )
                {
                    txt.text = text;
                    return true;
                }
            }

            return false;
        }

        public bool SetColor( string componentName, Color color, bool internalTxt = false )
        {
            GameObject go;
            if ( components.TryGetValue( componentName, out go ) )
            {
                Graphic graphic = internalTxt ? go.GetComponentInChildren<Graphic>() : go.GetComponent<Graphic>();
                if ( graphic )
                {
                    graphic.color = color;
                    return true;
                }
            }

            return false;
        }

        public bool SetInputFiledText( string componentName, string text, bool internalTxt = false )
        {
            GameObject go;
            if ( components.TryGetValue( componentName, out go ) )
            {
                InputField txt = internalTxt ? go.GetComponentInChildren<InputField>() : go.GetComponent<InputField>();
                if ( txt )
                {
                    txt.text = text;
                    return true;
                }
            }

            return false;
        }

        public string GetText( string componentName, bool internalTxt = false )
        {
            GameObject go;
            if ( components.TryGetValue( componentName, out go ) )
            {
                Text txt = internalTxt ? go.GetComponentInChildren<Text>() : go.GetComponent<Text>();
                if ( txt )
                {
                    return txt.text;
                }
            }

            return string.Empty;
        }

        public bool SetImageSprite( string componentName, Sprite sprite, bool internalImg = false, bool activate = true, bool setNative = false )
        {
            GameObject go;
            
            if ( components.TryGetValue( componentName, out go ) )
            {
                //Image img = internalImg ? go.GetComponentInChildren<Image>() : go.GetComponent<Image>();
                Image img = go.GetComponent<Image>();
                if ( internalImg )
                {
                    //Image[] imgs = go.GetComponentsInChildren<Image>();
                    //if ( imgs != null && imgs.Length > 1 )
                    //{
                    //    if ( img == imgs[0] )
                    //        img = imgs[1];
                    //}
                    
                    if ( go.transform.childCount > 0 )
                        img = go.transform.GetChild(0).GetComponent<Image>();
                }

                if ( img )
                {
                    img.sprite = sprite;

                    if ( setNative )
                        img.SetNativeSize();

                    if ( activate )
                        go.gameObject.SetActive( true );
                    return true;
                }
            }

            return false;
        }

        public void SetImageNaitve( string componentName, bool internalImg = false )
        {
            GameObject go;
            
            if ( components.TryGetValue( componentName, out go ) )
            {
                Image img = go.GetComponent<Image>();
                if ( internalImg )
                {
                    if ( go.transform.childCount > 0 )
                        img = go.transform.GetChild(0).GetComponent<Image>();
                }

                if ( img )
                    img.SetNativeSize();
            }
        }

        public Sprite GetImageSprite( string componentName, bool internalImg = false )
        {
            GameObject go;
            
            if ( components.TryGetValue( componentName, out go ) )
            {
                //Image img = internalImg ? go.GetComponentInChildren<Image>() : go.GetComponent<Image>();
                Image img = go.GetComponent<Image>();
                if ( internalImg )
                {
                    if ( go.transform.childCount > 0 )
                        img = go.transform.GetChild(0).GetComponent<Image>();
                }

                if ( img )
                {
                    return img.sprite;
                }
            }

            return null;
        }

        public void SetInteractables( bool val )
        {
            foreach ( var item in components )
            {
                var btn = item.Value.GetComponent<Button>();
                if ( btn )
                    btn.interactable = val;
            }
        }

        public void SetPosition( string name, Vector2 pos, bool show = false )
        {
            GameObject component = null;
            if ( components.TryGetValue( name, out component ) )
            {
                component.GetComponent<RectTransform>().position = pos;
                component.SetActive( show );
            }
            else
                Debug.Log( name + " couldn't be found in " + root.name );
        }

        public GameObject GetRoot()
        {
            return root;
        }
    }
}