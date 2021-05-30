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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""name"": ""NumpadMove"",
                    ""id"": ""9740a263-1357-4452-bf51-4424fe35e031"",
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
                    ""id"": ""f6960d9e-36e7-4c0b-bb3e-277d268192d1"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f9f8e072-fcc5-4f47-835e-c39532b07568"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""feaeccbc-05ca-4e14-8af8-ed738c71dfd6"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d2a43eaf-1ee5-48e3-ad6c-445cda67d30c"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""99e91d91-3e28-49b2-bba6-e5ba2903fbb3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse Default;PilotKeyboardMouse"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""id"": ""51023b4e-bcad-4bef-bd02-6f75ef5db9b6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                    ""id"": ""8edd9e82-c27f-4e4c-b3c2-f0094df4fffd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
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
                    ""groups"": ""Keyboard&Mouse Default"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""13536876-62a3-4fe8-bb0a-7cd3f6f38aa8"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""ChangeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ElectronicWarfareOfficer"",
            ""id"": ""e0646344-bfa8-42c0-94c9-03ea77a5ae43"",
            ""actions"": [
                {
                    ""name"": ""IncreaseMinimapResolution"",
                    ""type"": ""Button"",
                    ""id"": ""3f024827-c976-4bf9-993b-316123425579"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DecreaseMinimapResolution"",
                    ""type"": ""Button"",
                    ""id"": ""7d695515-08ba-48c6-92a0-a10f25d84bd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""829933d3-4ec6-4794-8342-7dd2822522b3"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""IncreaseMinimapResolution"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ac5ece7-2953-4404-ba02-26c1c14cf054"",
                    ""path"": ""<Keyboard>/comma"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PilotKeyboardMouse"",
                    ""action"": ""DecreaseMinimapResolution"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse Default"",
            ""bindingGroup"": ""Keyboard&Mouse Default"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
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
        },
        {
            ""name"": ""PilotKeyboardMouse"",
            ""bindingGroup"": ""PilotKeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
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
        // ElectronicWarfareOfficer
        m_ElectronicWarfareOfficer = asset.FindActionMap("ElectronicWarfareOfficer", throwIfNotFound: true);
        m_ElectronicWarfareOfficer_IncreaseMinimapResolution = m_ElectronicWarfareOfficer.FindAction("IncreaseMinimapResolution", throwIfNotFound: true);
        m_ElectronicWarfareOfficer_DecreaseMinimapResolution = m_ElectronicWarfareOfficer.FindAction("DecreaseMinimapResolution", throwIfNotFound: true);
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

    // ElectronicWarfareOfficer
    private readonly InputActionMap m_ElectronicWarfareOfficer;
    private IElectronicWarfareOfficerActions m_ElectronicWarfareOfficerActionsCallbackInterface;
    private readonly InputAction m_ElectronicWarfareOfficer_IncreaseMinimapResolution;
    private readonly InputAction m_ElectronicWarfareOfficer_DecreaseMinimapResolution;
    public struct ElectronicWarfareOfficerActions
    {
        private @Mech_Controller m_Wrapper;
        public ElectronicWarfareOfficerActions(@Mech_Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @IncreaseMinimapResolution => m_Wrapper.m_ElectronicWarfareOfficer_IncreaseMinimapResolution;
        public InputAction @DecreaseMinimapResolution => m_Wrapper.m_ElectronicWarfareOfficer_DecreaseMinimapResolution;
        public InputActionMap Get() { return m_Wrapper.m_ElectronicWarfareOfficer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ElectronicWarfareOfficerActions set) { return set.Get(); }
        public void SetCallbacks(IElectronicWarfareOfficerActions instance)
        {
            if (m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface != null)
            {
                @IncreaseMinimapResolution.started -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnIncreaseMinimapResolution;
                @IncreaseMinimapResolution.performed -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnIncreaseMinimapResolution;
                @IncreaseMinimapResolution.canceled -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnIncreaseMinimapResolution;
                @DecreaseMinimapResolution.started -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnDecreaseMinimapResolution;
                @DecreaseMinimapResolution.performed -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnDecreaseMinimapResolution;
                @DecreaseMinimapResolution.canceled -= m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface.OnDecreaseMinimapResolution;
            }
            m_Wrapper.m_ElectronicWarfareOfficerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @IncreaseMinimapResolution.started += instance.OnIncreaseMinimapResolution;
                @IncreaseMinimapResolution.performed += instance.OnIncreaseMinimapResolution;
                @IncreaseMinimapResolution.canceled += instance.OnIncreaseMinimapResolution;
                @DecreaseMinimapResolution.started += instance.OnDecreaseMinimapResolution;
                @DecreaseMinimapResolution.performed += instance.OnDecreaseMinimapResolution;
                @DecreaseMinimapResolution.canceled += instance.OnDecreaseMinimapResolution;
            }
        }
    }
    public ElectronicWarfareOfficerActions @ElectronicWarfareOfficer => new ElectronicWarfareOfficerActions(this);
    private int m_KeyboardMouseDefaultSchemeIndex = -1;
    public InputControlScheme KeyboardMouseDefaultScheme
    {
        get
        {
            if (m_KeyboardMouseDefaultSchemeIndex == -1) m_KeyboardMouseDefaultSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse Default");
            return asset.controlSchemes[m_KeyboardMouseDefaultSchemeIndex];
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
    private int m_PilotKeyboardMouseSchemeIndex = -1;
    public InputControlScheme PilotKeyboardMouseScheme
    {
        get
        {
            if (m_PilotKeyboardMouseSchemeIndex == -1) m_PilotKeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("PilotKeyboardMouse");
            return asset.controlSchemes[m_PilotKeyboardMouseSchemeIndex];
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
    public interface IElectronicWarfareOfficerActions
    {
        void OnIncreaseMinimapResolution(InputAction.CallbackContext context);
        void OnDecreaseMinimapResolution(InputAction.CallbackContext context);
    }
}
