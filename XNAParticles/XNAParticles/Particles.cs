using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

public class Particles
{
    private List<Particle> particles;
    private Effect fxParticles;
    private Random rnd;


    private float attraction = 0.0f;
    private float particleRadius = 0.25f;

    public float Damping { get; set; }
    public float Spring { get; set; }
    public float Shear { get; set; }
    public float Attraction
    {
        get { return attraction; }
        set
        {
            if (value < 0) value = 0;
            if (value > 0.1f) value = 0.1f;
            attraction = value;
        }
    }
    public float BoundaryDamping { get; set; }
    public Vector3 Gravity { get; set; }
    public float GlobalDamping { get; set; }
    public float ParticleRadius
    {
        get { return particleRadius; }
        set
        {
            fxParticles.Parameters["Size"].SetValue(particleRadius = value);
        }
    }


    public Particles(Effect fxParticles)
    {
        Gravity = new Vector3(0f, -3f, 0f);
        Damping = 0.02f;
        Spring = 0.5f;
        Shear = 0.1f;
        BoundaryDamping = -0.5f;
        GlobalDamping = 1f;

        particles = new List<Particle>();

        this.fxParticles = fxParticles;
        this.fxParticles.CurrentTechnique = fxParticles.Techniques[0];
        this.fxParticles.Parameters["Size"].SetValue(particleRadius);

        rnd = new Random();
    }

    public void Add(Particle p)
    {
        p.Velocity = new Vector3(
            rnd.Next(1, 2) * 0.01f,
            rnd.Next(1, 2) * 0.01f,
            rnd.Next(1, 2) * 0.01f);
        this.particles.Add(p);
    }
    public void Add(Vector3 pos)
    {
        this.Add(new Particle(pos, Color.White));
    }


    public void Update(float dt)
    {
        foreach (Particle p in particles)
        {
            Vector3 pos = p.Position;
            Vector3 vel = p.Velocity;

            vel += Gravity * dt;
            vel *= GlobalDamping;

            // new position = old position + velocity * deltatime
            pos += vel * dt;

            // Collision with bounds
            if (pos.X > 10.0f - particleRadius) { pos.X = 10.0f - particleRadius; vel.X *= BoundaryDamping; }
            if (pos.X < -10.0f + particleRadius) { pos.X = -10.0f + particleRadius; vel.X *= BoundaryDamping; }

            if (pos.Y < -10.0f + particleRadius) { pos.Y = -10.0f + particleRadius; vel.Y *= BoundaryDamping; }
            if (pos.Y > 10.0f + particleRadius) { pos.Y = 10.0f - particleRadius; vel.Y *= BoundaryDamping; }

            if (pos.Z > 10.0f - particleRadius) { pos.Z = 10.0f - particleRadius; vel.Z *= BoundaryDamping; }
            if (pos.Z < -10.0f + particleRadius) { pos.Z = -10.0f + particleRadius; vel.Z *= BoundaryDamping; }


            p.Position = pos;
            p.Velocity = vel;
        }

        ProcessCollisions();
    }

    public void ProcessCollisions()
    {
        foreach (Particle p in particles)
        {
            Vector3 force = Vector3.Zero;

            foreach (Particle p2 in particles)
            {
                if (p == p2)
                    continue;

                force += CollideSphere(p, p2);
            }

            p.Velocity += force;
        }
    }

    public void Draw(Matrix Projection, Matrix View, Vector3 Side, Vector3 Up)
    {
        fxParticles.Parameters["Projection"].SetValue(Projection);
        fxParticles.Parameters["View"].SetValue(View);
        fxParticles.Parameters["CameraSide"].SetValue(Side);
        fxParticles.Parameters["CameraUp"].SetValue(Up);
        fxParticles.CurrentTechnique.Passes[0].Apply();

        foreach (Particle p in particles)
            p.Draw(fxParticles);
    }


    private Vector3 CollideSphere(Particle pA, Particle pB)
    {
        Vector3 relPos = pB.Position - pA.Position;

        float dist = relPos.Length();
        float collideDist = particleRadius * 2;

        Vector3 force = Vector3.Zero;
        if (dist < collideDist)
        {
            if (dist == 0) dist = particleRadius;
            Vector3 norm = relPos / dist;

            Vector3 relVel = pB.Velocity - pA.Velocity;

            Vector3 tanVel = relVel - (Vector3.Dot(relVel, norm) * norm);

            force = -Spring * (collideDist - dist) * norm;
            force += Damping * relVel;
            force += Shear * tanVel;
            force += attraction * relPos;
        }

        return force;
    }
}
