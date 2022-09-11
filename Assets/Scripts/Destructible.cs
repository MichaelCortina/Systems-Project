using UnityEngine;
using Photon.Pun;
public class Destructible : MonoBehaviourPunCallbacks
{
    private bool _destroyNextFrame; //to eliminate multiple conflicting destroy calls
    
    [PunRPC]
    protected void DestroyThis()
    {
        _destroyNextFrame = true;
    }
    
    protected virtual void FixedUpdate() 
    {
        if (_destroyNextFrame) 
            Destroy(gameObject);
    }
}