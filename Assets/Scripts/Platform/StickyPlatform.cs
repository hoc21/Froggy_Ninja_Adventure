using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
  void OnCollisionEnter2D(Collision2D other)
  {
    if(other.gameObject.name == "Player")
    {
        other.gameObject.transform.SetParent(transform);
    }
  }
  void OnCollisionExit2D(Collision2D other)
  {
    if(other.gameObject.name == "Player")
    {
        other.gameObject.transform.SetParent(null);
    }
  }
}
