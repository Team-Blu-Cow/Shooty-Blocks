// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Input/MasterInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MasterInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MasterInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MasterInput"",
    ""maps"": [
        {
            ""name"": ""BasicKBM"",
            ""id"": ""412fc57f-054c-4174-b88f-24882327974b"",
            ""actions"": [
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""cfdbf7e9-cb1e-40d4-af8f-e0ef25f7363d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LClick"",
                    ""type"": ""Button"",
                    ""id"": ""77b5e3bb-729c-4fbd-9072-6ec5c6cbb0ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RClick"",
                    ""type"": ""Button"",
                    ""id"": ""e8d2e7b6-a3e3-410d-a1d4-ee5208dadafa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Directions"",
                    ""type"": ""Value"",
                    ""id"": ""dd56fd38-645e-49b9-ae4d-0316a2b19692"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FingerTouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""eea460ac-2550-4b3d-8a42-c48888338393"",
                    ""expectedControlType"": ""Touch"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32084a50-7fb0-47bf-b84b-c14b87925bfb"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58e872cb-0679-4f82-8dbd-c31d9ad497cc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""LClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99ab34a4-8378-4e62-8eaa-4519a40f6ab8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""RClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""wasd"",
                    ""id"": ""a85e98c5-4be1-4e0a-b08d-6a2bb442ee32"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Directions"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bb86c148-07df-457c-993a-fe7d9e12887f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""517c7b0c-443e-46f0-aaa9-1e815076762f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4e6a52ac-1b22-4914-8198-1701c4069f65"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f635e102-0a7f-4cb6-a580-65f208e4e967"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""f6b447bb-2e8c-4405-9222-7bc5a69c6107"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Directions"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""be1a9ece-748c-44a9-b3a4-1435927bbf70"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f2c05268-bed8-4862-bd04-408c5297c6ea"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0bdb9c4c-5a52-484d-a4fc-30733498d2f8"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""64149a1e-f0a0-4d30-af6a-0bd280a4f473"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Directions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard+Mouse"",
            ""bindingGroup"": ""Keyboard+Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BasicKBM
        m_BasicKBM = asset.FindActionMap("BasicKBM", throwIfNotFound: true);
        m_BasicKBM_MousePos = m_BasicKBM.FindAction("MousePos", throwIfNotFound: true);
        m_BasicKBM_LClick = m_BasicKBM.FindAction("LClick", throwIfNotFound: true);
        m_BasicKBM_RClick = m_BasicKBM.FindAction("RClick", throwIfNotFound: true);
        m_BasicKBM_Directions = m_BasicKBM.FindAction("Directions", throwIfNotFound: true);
        m_BasicKBM_FingerTouch = m_BasicKBM.FindAction("FingerTouch", throwIfNotFound: true);
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

    // BasicKBM
    private readonly InputActionMap m_BasicKBM;
    private IBasicKBMActions m_BasicKBMActionsCallbackInterface;
    private readonly InputAction m_BasicKBM_MousePos;
    private readonly InputAction m_BasicKBM_LClick;
    private readonly InputAction m_BasicKBM_RClick;
    private readonly InputAction m_BasicKBM_Directions;
    private readonly InputAction m_BasicKBM_FingerTouch;
    public struct BasicKBMActions
    {
        private @MasterInput m_Wrapper;
        public BasicKBMActions(@MasterInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePos => m_Wrapper.m_BasicKBM_MousePos;
        public InputAction @LClick => m_Wrapper.m_BasicKBM_LClick;
        public InputAction @RClick => m_Wrapper.m_BasicKBM_RClick;
        public InputAction @Directions => m_Wrapper.m_BasicKBM_Directions;
        public InputAction @FingerTouch => m_Wrapper.m_BasicKBM_FingerTouch;
        public InputActionMap Get() { return m_Wrapper.m_BasicKBM; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicKBMActions set) { return set.Get(); }
        public void SetCallbacks(IBasicKBMActions instance)
        {
            if (m_Wrapper.m_BasicKBMActionsCallbackInterface != null)
            {
                @MousePos.started -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnMousePos;
                @LClick.started -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnLClick;
                @LClick.performed -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnLClick;
                @LClick.canceled -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnLClick;
                @RClick.started -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnRClick;
                @RClick.performed -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnRClick;
                @RClick.canceled -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnRClick;
                @Directions.started -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnDirections;
                @Directions.performed -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnDirections;
                @Directions.canceled -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnDirections;
                @FingerTouch.started -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnFingerTouch;
                @FingerTouch.performed -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnFingerTouch;
                @FingerTouch.canceled -= m_Wrapper.m_BasicKBMActionsCallbackInterface.OnFingerTouch;
            }
            m_Wrapper.m_BasicKBMActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
                @LClick.started += instance.OnLClick;
                @LClick.performed += instance.OnLClick;
                @LClick.canceled += instance.OnLClick;
                @RClick.started += instance.OnRClick;
                @RClick.performed += instance.OnRClick;
                @RClick.canceled += instance.OnRClick;
                @Directions.started += instance.OnDirections;
                @Directions.performed += instance.OnDirections;
                @Directions.canceled += instance.OnDirections;
                @FingerTouch.started += instance.OnFingerTouch;
                @FingerTouch.performed += instance.OnFingerTouch;
                @FingerTouch.canceled += instance.OnFingerTouch;
            }
        }
    }
    public BasicKBMActions @BasicKBM => new BasicKBMActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard+Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IBasicKBMActions
    {
        void OnMousePos(InputAction.CallbackContext context);
        void OnLClick(InputAction.CallbackContext context);
        void OnRClick(InputAction.CallbackContext context);
        void OnDirections(InputAction.CallbackContext context);
        void OnFingerTouch(InputAction.CallbackContext context);
    }
}
