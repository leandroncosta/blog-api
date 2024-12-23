import 'package:flutter/material.dart';
import 'package:flutter_application/pages/login_page.dart';

void main() { //add this
    runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      home: LoginPage(),
    );
  }
}