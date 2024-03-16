//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/NavigationInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @NavigationInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @NavigationInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NavigationInputs"",
    ""maps"": [
        {
            ""name"": ""CameraControlMap"",
            ""id"": ""e0dc4522-073c-4d3c-be30-ca8bc33dc846"",
            ""actions"": [
                {
                    ""name"": ""InputMoveAction"",
                    ""type"": ""Value"",
                    ""id"": ""7f1a8169-b07c-49a4-b8cf-836ac2744681"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputPositionAction"",
                    ""type"": ""Value"",
                    ""id"": ""4e5b03ec-83a5-4738-8493-bfe83aa038a6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputPagingAction"",
                    ""type"": ""Value"",
                    ""id"": ""f5955579-1f26-4836-94b7-8cd347b32ecd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputPressAction2"",
                    ""type"": ""Button"",
                    ""id"": ""e1e35471-1912-4597-b8bc-0f91694d810c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputPressAction3"",
                    ""type"": ""Button"",
                    ""id"": ""2f9b8c4a-968a-42bb-a7ba-2f331aed915d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9992c0ce-f87c-4302-8a86-6b36dd41957a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMoveAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""497b69f6-b47b-421d-8d06-56ef4d0b91de"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPositionAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac5bbdee-1d4b-47ae-b661-9b92f9e7cf5e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPressAction2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f060795-6280-4a02-8d04-dfe6b1ee5dd0"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPressAction3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95bd8296-be1a-433c-9876-2d0c00ed2630"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPagingAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControlMap
        m_CameraControlMap = asset.FindActionMap("CameraControlMap", throwIfNotFound: true);
        m_CameraControlMap_InputMoveAction = m_CameraControlMap.FindAction("InputMoveAction", throwIfNotFound: true);
        m_CameraControlMap_InputPositionAction = m_CameraControlMap.FindAction("InputPositionAction", throwIfNotFound: true);
        m_CameraControlMap_InputPagingAction = m_CameraControlMap.FindAction("InputPagingAction", throwIfNotFound: true);
        m_CameraControlMap_InputPressAction2 = m_CameraControlMap.FindAction("InputPressAction2", throwIfNotFound: true);
        m_CameraControlMap_InputPressAction3 = m_CameraControlMap.FindAction("InputPressAction3", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CameraControlMap
    private readonly InputActionMap m_CameraControlMap;
    private List<ICameraControlMapActions> m_CameraControlMapActionsCallbackInterfaces = new List<ICameraControlMapActions>();
    private readonly InputAction m_CameraControlMap_InputMoveAction;
    private readonly InputAction m_CameraControlMap_InputPositionAction;
    private readonly InputAction m_CameraControlMap_InputPagingAction;
    private readonly InputAction m_CameraControlMap_InputPressAction2;
    private readonly InputAction m_CameraControlMap_InputPressAction3;
    public struct CameraControlMapActions
    {
        private @NavigationInputs m_Wrapper;
        public CameraControlMapActions(@NavigationInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @InputMoveAction => m_Wrapper.m_CameraControlMap_InputMoveAction;
        public InputAction @InputPositionAction => m_Wrapper.m_CameraControlMap_InputPositionAction;
        public InputAction @InputPagingAction => m_Wrapper.m_CameraControlMap_InputPagingAction;
        public InputAction @InputPressAction2 => m_Wrapper.m_CameraControlMap_InputPressAction2;
        public InputAction @InputPressAction3 => m_Wrapper.m_CameraControlMap_InputPressAction3;
        public InputActionMap Get() { return m_Wrapper.m_CameraControlMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlMapActions set) { return set.Get(); }
        public void AddCallbacks(ICameraControlMapActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraControlMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraControlMapActionsCallbackInterfaces.Add(instance);
            @InputMoveAction.started += instance.OnInputMoveAction;
            @InputMoveAction.performed += instance.OnInputMoveAction;
            @InputMoveAction.canceled += instance.OnInputMoveAction;
            @InputPositionAction.started += instance.OnInputPositionAction;
            @InputPositionAction.performed += instance.OnInputPositionAction;
            @InputPositionAction.canceled += instance.OnInputPositionAction;
            @InputPagingAction.started += instance.OnInputPagingAction;
            @InputPagingAction.performed += instance.OnInputPagingAction;
            @InputPagingAction.canceled += instance.OnInputPagingAction;
            @InputPressAction2.started += instance.OnInputPressAction2;
            @InputPressAction2.performed += instance.OnInputPressAction2;
            @InputPressAction2.canceled += instance.OnInputPressAction2;
            @InputPressAction3.started += instance.OnInputPressAction3;
            @InputPressAction3.performed += instance.OnInputPressAction3;
            @InputPressAction3.canceled += instance.OnInputPressAction3;
        }

        private void UnregisterCallbacks(ICameraControlMapActions instance)
        {
            @InputMoveAction.started -= instance.OnInputMoveAction;
            @InputMoveAction.performed -= instance.OnInputMoveAction;
            @InputMoveAction.canceled -= instance.OnInputMoveAction;
            @InputPositionAction.started -= instance.OnInputPositionAction;
            @InputPositionAction.performed -= instance.OnInputPositionAction;
            @InputPositionAction.canceled -= instance.OnInputPositionAction;
            @InputPagingAction.started -= instance.OnInputPagingAction;
            @InputPagingAction.performed -= instance.OnInputPagingAction;
            @InputPagingAction.canceled -= instance.OnInputPagingAction;
            @InputPressAction2.started -= instance.OnInputPressAction2;
            @InputPressAction2.performed -= instance.OnInputPressAction2;
            @InputPressAction2.canceled -= instance.OnInputPressAction2;
            @InputPressAction3.started -= instance.OnInputPressAction3;
            @InputPressAction3.performed -= instance.OnInputPressAction3;
            @InputPressAction3.canceled -= instance.OnInputPressAction3;
        }

        public void RemoveCallbacks(ICameraControlMapActions instance)
        {
            if (m_Wrapper.m_CameraControlMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraControlMapActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraControlMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraControlMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraControlMapActions @CameraControlMap => new CameraControlMapActions(this);
    public interface ICameraControlMapActions
    {
        void OnInputMoveAction(InputAction.CallbackContext context);
        void OnInputPositionAction(InputAction.CallbackContext context);
        void OnInputPagingAction(InputAction.CallbackContext context);
        void OnInputPressAction2(InputAction.CallbackContext context);
        void OnInputPressAction3(InputAction.CallbackContext context);
    }
}
