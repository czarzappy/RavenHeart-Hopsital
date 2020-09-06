using System;
using System.Collections.Generic;
using UnityEngine;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class JuiceSpray : MonoBehaviour
    {
        public ParticleSystem ps;

        public List<ParticleCollisionEvent> collisionEvents;
        // Start is called before the first frame update
        void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            throw new NotImplementedException();
        }
    
        void OnParticleCollision(GameObject other)
        {
            Debug.Log("Particle collided!");
            collisionEvents = new List<ParticleCollisionEvent>();
            int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

            Rigidbody rb = other.GetComponent<Rigidbody>();
            int i = 0;

            while (i < numCollisionEvents)
            {
                if (rb)
                {
                    Vector3 pos = collisionEvents[i].intersection;
                    Vector3 force = collisionEvents[i].velocity * 10;
                    rb.AddForce(force);
                }
                i++;
            }
        }

        void OnParticleTrigger()
        {
            // Debug.Log("Particle trigger!");

            // particles
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        
            // get
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            foreach (ParticleSystem.Particle particle in enter)
            {
                // particle.position.;
            }
        }
    }
}
