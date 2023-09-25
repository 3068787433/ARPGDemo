using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ARPGDemo.Character
{
    public interface IOnDamager
    {
        /// <summary>
        /// 受击
        /// </summary>
        void OnDamage(int damageVal);
    }
}
