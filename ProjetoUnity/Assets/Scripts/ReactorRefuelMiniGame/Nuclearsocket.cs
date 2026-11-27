using UnityEngine;

public class Nuclearsocket : MonoBehaviour
{
    public bool isVisited = false;
    public bool isLoaded = false;
    // Mudamos para [SerializeField] para você arrastar direto na Unity
    [Header("Configure arrastando os objetos da cena:")]
    [SerializeField] private GameObject oldPellet; // Pastilha Velha
    [SerializeField] private GameObject newPellet; // Pastilha Carregada


    public void ResetSocket()
    {
        isVisited = false;
        isLoaded = false;

        oldPellet.SetActive(true);
        newPellet.SetActive(false);

        Debug.Log("RESETOU UM PELLET, TUDO CERTO");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Claw"))
        {
            if (!collider.TryGetComponent(out ClawMovement claw))
                return;
        
            if (isVisited || claw.isHoldingNuclear)
                return;
        
            Debug.Log("Got used Pellet");
            isVisited = true;
            
            if (oldPellet != null) oldPellet.SetActive(false);
        
            if (claw.fixedNuclearPallet != null)
            {
                claw.fixedNuclearPallet.gameObject.SetActive(true);
            }
        
            claw.isHoldingNuclear = true;
            claw.holdingTreatedPellet = false; 
            return;
        }

        if (collider.CompareTag("LoadedPellet"))
        {
            if (isLoaded)
            {
                Destroy(collider.gameObject);
                return;
            }

            if (newPellet != null) newPellet.SetActive(true);
            isLoaded = true;
            
            if (ReloadController.Instance != null)
            {
                ReloadController.Instance.ReloadPellet();
            }


            Destroy(collider.gameObject);
        }
    }
}