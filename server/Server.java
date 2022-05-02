/*
 * Simple message relay server you could use for almost anything
 * A  popular specific use case is for chat/multiplayer games
 */

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Server extends Thread {

  private ServerSocket serverPortListener = null;
  private static ArrayList<PrintWriter> printWriterList;

  // Set up our listener port
  Server() throws IOException {
    serverPortListener = new ServerSocket(7520);
    printWriterList = new ArrayList<PrintWriter>();
  }

  public void run() {
    while (true) {
      try {
        // Always connect the user and setup a BufferReader & PrintWriter for it
        Socket serverPortSocket = serverPortListener.accept();
        BufferedReader input = new BufferedReader(new InputStreamReader(serverPortSocket.getInputStream()));
        PrintWriter output = new PrintWriter(serverPortSocket.getOutputStream(), true);

        // Pass these to our function below
        new Read(input, output).start();
      } catch (Exception e) {
        System.out.println(e.toString());
      }
    }
  }

  // Start this on a new thread
  public static void main(String[] args) throws IOException {
    Thread t = new Server();
    t.start();
  }

  // Send the message, but not to the sender
  public void broadcast(String message, PrintWriter out) {
    for (PrintWriter pw : printWriterList) {
      //if (pw != out) {
        pw.println(message);

        System.out.println("Sending: "+message);
      //}
    }
  }

  /*
   * This is the main class that handles the input
   * Run on a new thread
   */
  class Read extends Thread {
    BufferedReader inp;
    PrintWriter out;
    String input;

    Read(BufferedReader inp, PrintWriter out) {
      this.inp = inp;
      this.out = out;
    }

    public void run() {
      try {
        printWriterList.add(out);

        // Receive input from the client
        // Kill the connection when recieved "end"
        while ((input = inp.readLine())!=null) {
          if (input!="") {
            if (input.equals("end")) {
              break;
            } else {
              broadcast(input, out);
            }
          }
        }

        // Remove this connection from the list
        for (int i = 0; i != printWriterList.size(); i++) {
          if (printWriterList.get(i).equals(out)) {
            printWriterList.remove(i);
          }
        }
      } catch (Exception ex) {
        System.err.println(ex.getMessage());
      }
    }
  }
}
