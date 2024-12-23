import 'package:flutter/material.dart';
import 'package:flutter_application/shared/widgets/register_user/text_top_widget.dart';
import 'package:flutter_application/shared/widgets/register_user/form_container_widget.dart';

class UserRegisterPage extends StatelessWidget {
  const UserRegisterPage({super.key});

  @override
  Widget build(BuildContext context) {
    return const SafeArea(
      child: Scaffold(
        backgroundColor: Colors.yellow,
        body: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [TextTopWidget(), FormContainerWidget()],
        ),
      ),
    );
  }
}
