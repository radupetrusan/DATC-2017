package com.example.tema1datc.tema1_datc;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {


    Button click,click1;
    public static TextView data;
    fetchData process = new fetchData();

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        click = (Button) findViewById(R.id.button_c);
        data = (TextView) findViewById(R.id.fetched_data);
        click1 = (Button) findViewById(R.id.button_c1);

        click1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MainActivity.data.setText(null);
                process.execute();
            }
        });

        click.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MainActivity.data.setText(null);
                process.execute();
            }


        });


    }
}
