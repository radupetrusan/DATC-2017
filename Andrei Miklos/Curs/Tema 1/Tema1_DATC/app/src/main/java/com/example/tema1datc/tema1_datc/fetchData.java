package com.example.tema1datc.tema1_datc;

import android.os.AsyncTask;
import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by Andrei on 10/8/2017.
 */

public class fetchData extends AsyncTask<Void,Void,Void> {
    String data = "";
    String dataParsed = "";
    String singleParsed = "";
    @Override
    protected Void doInBackground(Void... voids) {
        try {
            URL url = new URL("https://datc-rest.azurewebsites.net/breweries");
            HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
            InputStream inputStream = httpURLConnection.getInputStream();
            BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
            String line = "";
            while(line != null) {
                line = bufferedReader.readLine();
                data = data + line;
            }

            JSONObject parentObject = new JSONObject(data);
            JSONArray parentArray = parentObject.getJSONArray("ResourceList");
            for(int i=0;i<parentArray.length();i++) {
                JSONObject finalObject = parentArray.getJSONObject(i);

                String id = finalObject.getString("Id");
                String name = finalObject.getString("Name");

                singleParsed = singleParsed + "Id:" + id + "\n" +
                        "Nume:" + name + "\n"+"\n";

            }

            dataParsed = dataParsed+singleParsed+"\n";


        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return null;
    }

    @Override
    protected void onPostExecute(Void aVoid) {
        super.onPostExecute(aVoid);
        MainActivity.data.setText(this.dataParsed);
    }
}
