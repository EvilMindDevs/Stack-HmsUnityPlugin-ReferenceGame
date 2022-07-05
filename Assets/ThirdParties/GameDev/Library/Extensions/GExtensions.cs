using UnityEngine;
using UnityEngine.UI;

namespace GameDev.Library
{

    /// <summary>
    /// Method extension in C# is a feature that gives an opportunity to extend built-in
    /// class by defining/adding new methods but wont allow overriding built-in methods.
    /// without needs of inheritance and using them like on its own.
    /// </summary>
    /// 

    static public class GExtensions
    {
        // Component Extensions
        static public T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }

            return result;
        }

        static public void AddListener<TEventData>(this Component child, string eventName, EventHandler<TEventData> handler)
        {
            if (GDispatcher.instance != null)
            {
                GDispatcher.instance.Add(eventName, child.gameObject, handler);
            }
        }

        static public void RemoveListener<TEventData>(this Component child, string eventName, EventHandler<TEventData> handler)
        {
            if (GDispatcher.instance != null)
            {
                GDispatcher.instance.Remove(eventName, child.gameObject, handler);
            }
        }

        static public void DispatchEvent<TEventData>(this Component child, GEvent<TEventData> e, bool localPropagation = false)
        {
            if (GDispatcher.instance != null)
            {
                GDispatcher.instance.Dispatch(child, e, localPropagation);
            }
        }

        static public void PropagateEvent<TEventData>(this Component child, GEvent<TEventData> e)
        {
            if (GDispatcher.instance != null)
            {
                GDispatcher.instance.Propagate(child, e);
            }
        }

        // Texture2D Extensions
        static public Sprite ConvertToSprite(this Texture2D texture)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, rect, new Vector2(0, 0), 100f);
        }

        #region (JOB)
        // object Extensions (JOB)

        //static public void RunMethod(this Component component, float delay, Scheduler.Job job)
        //{
        //    if (Scheduler.instance != null)
        //    {
        //        Scheduler.instance.RunMethod(component.gameObject, delay, job);
        //    }
        //}

        //static public void RunNextFrame(this Component component, Scheduler.Job job)
        //{
        //    if (Scheduler.instance != null)
        //    {
        //        Scheduler.instance.RunMethod(component.gameObject, 0.0f, job);
        //    }
        //}

        //static public void StopMethod(this Component component)
        //{
        //    if (Scheduler.instance != null)
        //    {
        //        Scheduler.instance.StopMethods(component.gameObject);
        //    }
        //} 
        #endregion

        // Image Extensions
        static public void SetAlpha(this Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        static public void SetAlpha(this Text text, float alpha)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }


        // RectTransform Extensions
        static public void SetScale(this RectTransform rectTransform, float scale)
        {
            rectTransform.localScale = Vector3.one * scale;
        }

        static public float SetX(this RectTransform rectTransform, float x)
        {
            Vector3 pos = rectTransform.position;
            float prevVal = pos.x;
            pos.x = x;
            rectTransform.position = pos;
            return prevVal;
        }

        static public float SetY(this RectTransform rectTransform, float y)
        {
            Vector3 pos = rectTransform.position;
            float prevVal = pos.y;
            pos.y = y;
            rectTransform.position = pos;
            return prevVal;

        }

        static public float SetZ(this RectTransform rectTransform, float z)
        {
            Vector3 pos = rectTransform.position;
            float prevVal = pos.z;
            pos.z = z;
            rectTransform.position = pos;
            return prevVal;
        }
    }
}


