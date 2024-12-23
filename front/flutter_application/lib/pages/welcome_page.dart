import 'package:flutter/material.dart';
import 'package:flutter_application/pages/login_page.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'home_page.dart';

class WelcomePage extends StatefulWidget {
  const WelcomePage({super.key});

  @override
  State<WelcomePage> createState() => _WelcomePageState();
}

class _WelcomePageState extends State<WelcomePage> {

  @override
  void initState() {
    super.initState();
    verifyToken().then((value) {
      if (value) {
        Navigator.pushReplacement(context, 
        MaterialPageRoute(builder: (context) => HomePage())
        );
      } else {
        Navigator.pushReplacement(context,
        MaterialPageRoute(builder: (context) =>  LoginPage())
        );
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      body: Center(
        child: CircularProgressIndicator(),
      )
    );
  }

  Future<bool> verifyToken() async {
    SharedPreferences sharedPreference = await SharedPreferences.getInstance();
    if (sharedPreference.getString('token') != null ){
      return true;
    } else {
      return false;
    }
  }
}