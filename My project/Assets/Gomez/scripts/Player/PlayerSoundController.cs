using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoRecibirDanio;
    public AudioClip sonidoCaida;
    public AudioClip sonidoMov1;
    public AudioClip sonidoMov2;
    public AudioClip sonidoDerrota;
    public AudioClip sonidoVictoria;


    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSaltar);
    }


    public void playRecibirDanio()
    {
        audioSource.PlayOneShot(sonidoRecibirDanio);
    }



    public void playsonidoCaida()
    {
        audioSource.PlayOneShot(sonidoCaida);
    }


    public void playsonidoMov1()
    {
        audioSource.PlayOneShot(sonidoMov1);
    }


    public void playsonidoMov2()
    {
        audioSource.PlayOneShot(sonidoMov2);
    }



    public void playsonidoDerrota()
    {
        audioSource.PlayOneShot(sonidoDerrota);
    }



    public void playsonidoVictoria()
    {
        audioSource.PlayOneShot(sonidoVictoria);
    }







}
