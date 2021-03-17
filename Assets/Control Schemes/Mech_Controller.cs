// GENERATED AUTOMATICALLY FROM 'Assets/Control Schemes/Mech_Controller.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Mech_Controller : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Mech_Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Mech_Controller"",
    ""maps"": [
        {
            ""name"": ""MechPilot"",
            ""id"": ""fe3a3948-7818-4a04-b434-88cd93b71bb7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""9cc7a52d-b185-4ff9-b301-555773088682"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire1"",
                    ""type"": ""Button"",
                    ""id"": ""62e951fd-2e4f-4ea9-a013-d185daf9d96d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""8802992a-152f-473c-b31c-e89182209155"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire2"",
                    ""type"": ""Button"",
                    ""id"": ""a3858add-b64b-41ab-8c26-0860c22ef78d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""f195206d-67de-4019-83f7-c0ed6de54023"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WSAD"",
                    ""id"": ""a20065fc-8983-43c8-88a0-f6436c21432d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4433e0f7-2d6f-424d-8d68-67408a5a019b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7b787468-527f-4f02-aff5-90f5cbdb6943"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dd4b7851-c7d3-49e1-805b-c39ffb194e1e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18ef1f3b-e446-4b47-ae7b-8d1d4703aaa1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2de82e5-2e43-4e4d-bf8f-c7829d457d6b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99e91d91-3e28-49b2-bba6-e5ba2903fbb3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fire1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c2e73f4-5905-4e90-bd66-5beb48b8c001"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Fire1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1a62d32-1fe1-47f1-a7f8-241463c952f9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a6a7afd-4d30-407e-8428-6b567b416148"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ba45e11-66f5-4262-8b10-45ff5d5bdd92"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fire2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0adf44b5-f525-49ac-90d8-4db05bf3baa2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Fire2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de6ef2cc-4bdb-4f12-a1cc-625e4d0e05ff"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ChangeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47df3803-ee56-4a4b-938c-364f0b30fee6"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""ChangeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
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
        },
        {
            ""name"": ""XboxController"",
            ""bindingGroup"": ""XboxController"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // MechPilot
        m_MechPilot = asset.FindActionMap("MechPilot", throwIfNotFound: true);
        m_MechPilot_Move = m_MechPilot.FindAction("Move", throwIfNotFound: true);
        m_MechPilot_Fire1 = m_MechPilot.FindAction("Fire1", throwIfNotFound: true);
        m_MechPilot_Look = m_MechPilot.FindAction("Look", throwIfNotFound: true);
        m_MechPilot_Fire2 = m_MechPilot.FindAction("Fire2", throwIfNotFound: true);
        m_MechPilot_ChangeWeapon = m_MechPilot.FindAction("ChangeWeapon", throwIfNotFound: true);
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

    // MechPilot
    private readonly InputActionMap m_MechPilot;
    private IMechPilotActions m_MechPilotActionsCallbackInterface;
    private readonly InputAction m_MechPilot_Move;
    private readonly InputAction m_MechPilot_Fire1;
    private readonly InputAction m_MechPilot_Look;
    private readonly InputAction m_MechPilot_Fire2;
    private readonly InputAction m_MechPilot_ChangeWeapon;
    public struct MechPilotActions
    {
        private @Mech_Controller m_Wrapper;
        public MechPilotActions(@Mech_Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_MechPilot_Move;
        public InputAction @Fire1 => m_Wrapper.m_MechPilot_Fire1;
        public InputAction @Look => m_Wrapper.m_MechPilot_Look;
        public InputAction @Fire2 => m_Wrapper.m_MechPilot_Fire2;
        public InputAction @ChangeWeapon => m_Wrapper.m_MechPilot_ChangeWeapon;
        public InputActionMap Get() { return m_Wrapper.m_MechPilot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MechPilotActions set) { return set.Get(); }
        public void SetCallbacks(IMechPilotActions instance)
        {
            if (m_Wrapper.m_MechPilotActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnMove;
                @Fire1.started -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire1;
                @Fire1.performed -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire1;
                @Fire1.canceled -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire1;
                @Look.started -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnLook;
                @Fire2.started -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire2;
                @Fire2.performed -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire2;
                @Fire2.canceled -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnFire2;
                @ChangeWeapon.started -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnChangeWeapon;
                @ChangeWeapon.performed -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnChangeWeapon;
                @ChangeWeapon.canceled -= m_Wrapper.m_MechPilotActionsCallbackInterface.OnChangeWeapon;
            }
            m_Wrapper.m_MechPilotActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Fire1.started += instance.OnFire1;
                @Fire1.performed += instance.OnFire1;
                @Fire1.canceled += instance.OnFire1;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Fire2.started += instance.OnFire2;
                @Fire2.performed += instance.OnFire2;
                @Fire2.canceled += instance.OnFire2;
                @ChangeWeapon.started += instance.OnChangeWeapon;
                @ChangeWeapon.performed += instance.OnChangeWeapon;
                @ChangeWeapon.canceled += instance.OnChangeWeapon;
            }
        }
    }
    public MechPilotActions @MechPilot => new MechPilotActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_XboxControllerSchemeIndex = -1;
    public InputControlScheme XboxControllerScheme
    {
        get
        {
            if (m_XboxControllerSchemeIndex == -1) m_XboxControllerSchemeIndex = asset.FindControlSchemeIndex("XboxController");
            return asset.controlSchemes[m_XboxControllerSchemeIndex];
        }
    }
    public interface IMechPilotActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnFire1(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire2(InputAction.CallbackContext context);
        void OnChangeWeapon(InputAction.CallbackContext context);
    }
}
