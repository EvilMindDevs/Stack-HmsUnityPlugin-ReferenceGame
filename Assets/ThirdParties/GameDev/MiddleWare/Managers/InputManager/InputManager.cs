using GameDev.Library;

using System.Collections;

using UnityEngine;

namespace GameDev.MiddleWare
{
    public class InputManager : GSingleton<InputManager>
    {

        //#region Initialize

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void Initialize()
        //{
        //    GLog.Log($"Initialize", GLogName.InputManager);

        //    var gameObject = new GameObject();
        //    gameObject.AddComponent<InputManager>();
        //    gameObject.name = "InputManager";
        //    DontDestroyOnLoad(gameObject);
        //}

        //#endregion

        //private Vector3 _currentTouchPosition;
        //private Vector3 _previosTouchPosition;
        //private Vector3 _direction;
        //private Vector3 _swipeDirection;

        //private bool _isPressed = false;
        //private bool _isMoving;
        //private Vector3 _velocityOfInput;
        //private bool _secondHasClicked;

        //private bool _isTouched = false;

        //#region Constants
        //private const int RESPONSIVE_COEFFICIENT = 1500;
        //private const float SWIPE_CONTROL_DURATION = 0.01F;
        //private const float SWIPE_CONTROL_DISTANCE = 0.000000001f;
        //private const float JOYSTIC_CONTROL_DISTANCE = 0.1f;
        //#endregion

        //public Vector3 SwipeDirection => _swipeDirection;
        //public bool IsPressed => _isPressed;



        //#region Unity: OnEnable

        //private void OnEnable()
        //{
        //    GLog.Log($"OnEnable", GLogName.InputManager);

        //    this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        //    this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);

        //    this.AddListener<object>(GEventName.MOUSE_BUTTON_DOWN, MouseButtonDown);
        //    this.AddListener<object>(GEventName.MOUSE_BUTTON_UP, MouseButtonUp);
        //}

        //#endregion

        //#region Unity: OnDisable

        //private void OnDisable()
        //{
        //    GLog.Log($"OnDisable", GLogName.InputManager);

        //    this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        //    this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);

        //    this.RemoveListener<object>(GEventName.MOUSE_BUTTON_DOWN, MouseButtonDown);
        //    this.RemoveListener<object>(GEventName.MOUSE_BUTTON_UP, MouseButtonUp);
        //}

        //#endregion

        //#region Unity: Update

        //private void Update()
        //{
        //    if (!GameManager.instance.SessionIsPlaying)
        //    {
        //        return;
        //    }

        //    InputTouch();
        //    InputJoystic();
        //}

        //#endregion



        //#region Input: Joystic

        //private void InputJoystic()
        //{
        //    if (Input.GetMouseButton(0))
        //    {

        //        if (_isPressed)
        //        {
        //            _currentTouchPosition = ResponsiveInput();

        //            if (!_secondHasClicked)
        //            {
        //                _secondHasClicked = true;
        //            }
        //        }

        //        else
        //        {
        //            _currentTouchPosition = ResponsiveInput();
        //            _previosTouchPosition = ResponsiveInput();
        //            _isPressed = true;
        //            _isMoving = true;

        //        }

        //        _direction = _currentTouchPosition - _previosTouchPosition;

        //        if (_direction.magnitude > JOYSTIC_CONTROL_DISTANCE)
        //        {
        //            if (!_isMoving)
        //            {
        //                _isMoving = true;
        //            }

        //            _velocityOfInput = Vector3.ClampMagnitude(_direction, JOYSTIC_CONTROL_DISTANCE);

        //            if (_secondHasClicked)
        //            {
        //                _previosTouchPosition += (_direction - _velocityOfInput);
        //            }

        //            _swipeDirection = _direction - _velocityOfInput;
        //        }

        //        else
        //        {
        //            if (_isMoving)
        //            {
        //                _isMoving = false;
        //                _swipeDirection = Vector3.zero;
        //            }
        //        }
        //    }

        //    else
        //    {
        //        if (_isPressed)
        //        {
        //            _isPressed = false;
        //            _swipeDirection = Vector3.zero;
        //        }
        //    }

        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        _isPressed = false;
        //        _isMoving = false;
        //        _secondHasClicked = false;
        //        _swipeDirection = Vector3.zero;
        //    }


        //}

        //#endregion

        //#region Input: Touch

        //private void InputTouch()
        //{
        //    if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        //    {
        //        if (!_isTouched)
        //        {
        //            _isTouched = true;

        //            this.DispatchEvent(new GEvent<object>(GEventName.MOUSE_BUTTON_DOWN, this));
        //        }
        //    }
        //    else
        //    {
        //        if (_isTouched)
        //        {
        //            _isTouched = false;
        //            this.DispatchEvent(new GEvent<object>(GEventName.MOUSE_BUTTON_UP, this));
        //        }
        //    }
        //}

        //#endregion

        //#region Input: Control

        //private IEnumerator InputControl()
        //{
        //    Vector3 previousPosition = Vector3.zero;

        //    while (true)
        //    {
        //        if (Vector3.Distance(previousPosition, UnityEngine.Input.mousePosition) < SWIPE_CONTROL_DISTANCE)
        //        {
        //            _swipeDirection = Vector3.zero;
        //        }

        //        previousPosition = UnityEngine.Input.mousePosition;

        //        yield return new WaitForSeconds(SWIPE_CONTROL_DURATION);
        //    }
        //}

        //#endregion

        //#region Input: Responsive

        //private Vector3 ResponsiveInput()
        //{
        //    //return Camera.main.ScreenToViewportPoint(UnityEngine.Input.mousePosition) * RESPONSIVE_COEFFICIENT;

        //    return Input.mousePosition;
        //}

        //#endregion



        //#region Events: OnSessionStart

        //private void OnSessionStart(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"OnSessionStart", GLogName.InputManager);


        //}

        //#endregion

        //#region Events: OnSessionEnd

        //private void OnSessionEnd(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"OnSessionEnd", GLogName.InputManager);

        //    StopAllCoroutines();
        //}

        //#endregion


        //#region Events: MouseButtonDown

        //private void MouseButtonDown(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"MouseButtonDown", GLogName.InputManager);

        //}

        //#endregion

        //#region Events: MouseButtonUp

        //private void MouseButtonUp(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"MouseButtonUp", GLogName.InputManager);
        //}

        //#endregion

    }
}




