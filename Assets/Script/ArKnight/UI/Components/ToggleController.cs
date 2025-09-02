using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI.Components {
    public class ToggleController : MonoBehaviour {
        [SerializeField] private Toggle toggle;
        
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();
        
        private void Awake() {
            if (toggle == null) {
                toggle = GetComponent<Toggle>();
            }
            
            if (toggle != null) {
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }
        }
        
        private void OnToggleValueChanged(bool value) {
            onValueChanged.Invoke(value);
        }
        
        public void Initialization(bool value) {
            if (toggle != null) {
                toggle.isOn = value;
            }
        }
        
        public bool GetValue() {
            return toggle != null ? toggle.isOn : false;
        }
        
        public void SetValue(bool value) {
            if (toggle != null) {
                toggle.isOn = value;
            }
        }
    }
} 