using UnityEngine;

public class NuclearTrash : MonoBehaviour
{
    public NuclearWasteTreatment nuclearWasteTreatment;
    private int collectedNuclearTrash = 0;
    public int numberOfNuclearSockets;

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Claw"))
        {
            if(collider.gameObject.TryGetComponent<ClawMovement>(out ClawMovement claw))
            {
                if(!claw.isHoldingNuclear) return;

                if (!claw.holdingTreatedPellet)
                {
                    Debug.LogWarning("Pastilha descartada incorretamente sem tratamento!");
                    ReloadController.Instance.RadiationIntoxication();
                }

                collectedNuclearTrash++;
                claw.fixedNuclearPallet.gameObject.SetActive(false);
                claw.isHoldingNuclear = false;
                
                ReloadController.Instance.DiscardPellet();
                Debug.Log("Touched the Trash");
            }
        }
    }
}