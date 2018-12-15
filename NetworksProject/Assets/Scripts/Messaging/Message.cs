using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {
    /** Messages are the most important aspect of the game
     * The player can only learn, see, find out anything by receiving a message
     * Messages collect data about their surroundings on creation,
     * and relay the data directly to the destination Network.
     * 
     * It is a network's job to forward messages onwards towards the capital.
     * By the time messages arrive, they may well present out of date data.
     */

    public Ghost[] ghosts;

    public Message(Ghost[] ghosts) {
        this.ghosts = ghosts;
    }
    
}
