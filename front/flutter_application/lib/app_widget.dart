import 'package:flutter/material.dart';
import 'package:flutter_application/pages/user_register/user_register_page.dart';

class AppWidget extends StatelessWidget {
  const AppWidget({super.key});

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      title: "teste",
      home: UserRegisterPage(),
    );
  }
}
