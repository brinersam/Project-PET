﻿namespace ProjectPet.SharedKernel;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}