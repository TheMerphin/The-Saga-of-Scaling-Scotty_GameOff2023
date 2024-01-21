using UnityEngine;

namespace Weapons
{
    public class WeaponAnimEventHandlerProxy : AnimEventHandlerProxy<Weapon>
    {
        private ItemManager _itemManager;

        private void Start()
        {
            _itemManager = GetComponent<ItemManager>();
        }

        public void ForwardWeaponAnimEvent(string functionName)
        {
            var weapon = _itemManager.GetSelectedItem() as Weapon;
            ForwardAnimEvent(weapon, functionName);
        }
    }
}
