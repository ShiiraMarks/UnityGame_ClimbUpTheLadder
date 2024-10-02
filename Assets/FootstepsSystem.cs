using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip grass; //
    public AudioClip stone; //
    public AudioClip sand; //
    public AudioClip mud; //
    public AudioClip metal; //
    public AudioClip wood; //
    public AudioClip snow; //

    RaycastHit hit;

    public Transform RayStart;
    public float range;
    public LayerMask layerMask;

    public void Footstep()
    {
        if (Physics.Raycast(RayStart.position, -RayStart.transform.up, out hit, range, layerMask))
        {
            if (hit.collider.CompareTag("f.grass"))
            {
                PlayFootstepSoundL(grass);
            }
            else if (hit.collider.CompareTag("f.stone"))
            {
                PlayFootstepSoundL(stone);
            }
            else if (hit.collider.CompareTag("f.sand"))
            {
                PlayFootstepSoundL(sand);
            }
            else if (hit.collider.CompareTag("f.mud"))
            {
                PlayFootstepSoundL(mud);
            }
            else if (hit.collider.CompareTag("f.metal"))
            {
                PlayFootstepSoundL(metal);
            }
            else if (hit.collider.CompareTag("f.wood"))
            {
                PlayFootstepSoundL(wood);
            }
            else if (hit.collider.CompareTag("f.snow"))
            {
                PlayFootstepSoundL(snow);
            }
        }
    }

    void PlayFootstepSoundL(AudioClip clip)
    {
        audioSource.pitch = Random.Range(0.8f, 1f); // Changed AudioSource to audioSource
        audioSource.PlayOneShot(clip); // Changed audio to clip
    }

    private void Update()
    {
        Debug.DrawRay(RayStart.position, RayStart.transform.up * range * -1, Color.green);
    }
}