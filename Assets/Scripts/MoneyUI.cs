
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    // Update is called once per frame
    void Update()
    {
        //ajout un $ devant la variable money
        //La variable money n'a pas vraiment besoin d'etre updater a chaque frame . mais dans notre cas , ca va eter ok.
        moneyText.text = "$" + PlayerStats.Money.ToString();
    }
}
